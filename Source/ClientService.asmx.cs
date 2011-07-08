// <copyright file="ClientService.asmx.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2010
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Survey
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Script.Services;
    using System.Web.Security;
    using System.Web.Services;

    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Host;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Portals;
    using DotNetNuke.Entities.Users;
    using DotNetNuke.Security;
    using DotNetNuke.Security.Permissions;
    using DotNetNuke.Security.Roles;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.Services.Personalization;

    using Engage.Survey.Entities;

    /// <summary>
    ///   Web services used on the client
    /// </summary>
    [WebService(Namespace = "http://services.engagesoftware.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class ClientService : WebService
    {
        /// <summary>
        ///   Deletes the question.
        /// </summary>
        /// <param name = "questionId">The question id.</param>
        [WebMethod]
        public void DeleteQuestion(int questionId)
        {
            var surveyRepository = new SurveyRepository();
            var moduleId = surveyRepository.GetModuleIdForQuestion(questionId);
            if (!this.CanEditModule(moduleId))
            {
                this.DenyAccess();
            }

            surveyRepository.DeleteQuestion(questionId);
        }

        /// <summary>
        ///   Deletes the survey.
        /// </summary>
        /// <param name = "surveyId">The survey ID.</param>
        [WebMethod]
        public void DeleteSurvey(int surveyId)
        {
            var surveyRepository = new SurveyRepository();
            var moduleId = surveyRepository.GetModuleIdForSurvey(surveyId);
            if (!this.CanEditModule(moduleId))
            {
                this.DenyAccess();
            }

            surveyRepository.DeleteSurvey(surveyId, moduleId);
        }

        /// <summary>
        ///   Reorders the questions for a given <see cref = "Survey" />.
        /// </summary>
        /// <param name = "surveyId">The ID of the <see cref = "Survey" /> to which the questions belong.</param>
        /// <param name = "questionOrderMap">A <see cref = "Dictionary{TKey,TValue}" /> mapping question IDs to relative order.</param>
        [WebMethod]
        public void ReorderQuestions(int surveyId, Dictionary<string, int> questionOrderMap)
        {
            var surveyRepository = new SurveyRepository();
            var moduleId = surveyRepository.GetModuleIdForSurvey(surveyId);
            if (!this.CanEditModule(moduleId))
            {
                this.DenyAccess();
            }

            var survey = surveyRepository.LoadSurvey(surveyId);

            foreach (var questionIdOrderPair in questionOrderMap)
            {
                var questionId = int.Parse(questionIdOrderPair.Key, CultureInfo.InvariantCulture);
                var relativeOrder = questionIdOrderPair.Value;
                survey.Sections[0].Questions.Where(q => q.QuestionId == questionId).Single().RelativeOrder =
                    relativeOrder;
            }

            surveyRepository.SubmitChanges();
        }

        /// <summary>
        ///   Inserts or updates the give <paramref name = "question" />.
        /// </summary>
        /// <param name = "surveyId">The ID of the <see cref = "Survey" /> that <paramref name = "question" /> belongs to.</param>
        /// <param name = "question">The question.</param>
        /// <returns>
        ///   The inserted question, with IDs for the question and answers
        /// </returns>
        [WebMethod]
        public object UpdateQuestion(int surveyId, Question question)
        {
            var surveyRepository = new SurveyRepository();
            var survey = surveyRepository.LoadSurvey(surveyId);
            if (!this.CanEditModule(survey.ModuleId))
            {
                this.DenyAccess();
            }

            Question questionToUpdate;
            if (question.QuestionId > 0)
            {
                questionToUpdate =
                    survey.Sections.First().Questions.Where(q => q.QuestionId == question.QuestionId).Single();
                questionToUpdate.RevisingUser = question.RevisingUser;
            }
            else
            {
                questionToUpdate = surveyRepository.CreateQuestion(question.RevisingUser);
                survey.Sections.First().Questions.Add(questionToUpdate);
            }

            questionToUpdate.Text = question.Text;
            questionToUpdate.IsRequired = question.IsRequired;
            questionToUpdate.RelativeOrder = question.RelativeOrder;
            questionToUpdate.ControlType = question.ControlType;

            var answersToDelete = from answer in questionToUpdate.Answers
                                  where !question.Answers.Any(a => a.AnswerId == answer.AnswerId)
                                  select answer;
            surveyRepository.DeleteAnswers(answersToDelete, true);

            int answerOrder = 0;
            foreach (var answer in question.Answers)
            {
                Answer answerToUpdate;
                if (answer.AnswerId > 0)
                {
                    var lambdaAnswer = answer;
                    answerToUpdate = questionToUpdate.Answers.Where(a => a.AnswerId == lambdaAnswer.AnswerId).Single();
                    answerToUpdate.RevisingUser = question.RevisingUser;
                }
                else
                {
                    answerToUpdate = surveyRepository.CreateAnswer(question.RevisingUser);
                    questionToUpdate.Answers.Add(answerToUpdate);
                }

                answerToUpdate.Text = answer.Text;
                answerToUpdate.RelativeOrder = ++answerOrder;
            }

            surveyRepository.SubmitChanges();

            return
                new
                    {
                        questionToUpdate.QuestionId, 
                        questionToUpdate.ControlType, 
                        questionToUpdate.RelativeOrder, 
                        questionToUpdate.Text, 
                        questionToUpdate.IsRequired, 
                        Answers =
                            questionToUpdate.Answers.OrderBy(a => a.RelativeOrder).Select(
                                a => new { a.AnswerId, a.RelativeOrder, a.Text })
                    };
        }

        /// <summary>
        ///   Inserts or updates the given <paramref name = "survey" />.
        /// </summary>
        /// <param name = "survey">The survey.</param>
        /// <returns>The ID of the survey</returns>
        [WebMethod]
        public int UpdateSurvey(Survey survey)
        {
            int moduleId;
            Survey surveyToUpdate;
            var surveyRepository = new SurveyRepository();
            if (survey.SurveyId > 0)
            {
                surveyToUpdate = surveyRepository.LoadSurvey(survey.SurveyId);
                surveyToUpdate.RevisingUser = surveyToUpdate.Sections[0].RevisingUser = survey.RevisingUser;
                moduleId = surveyToUpdate.ModuleId;
            }
            else
            {
                surveyToUpdate = surveyRepository.CreateSurvey(survey.RevisingUser, survey.PortalId, survey.ModuleId);
                moduleId = survey.ModuleId;
            }

            if (!this.CanEditModule(moduleId))
            {
                this.DenyAccess();
            }

            // TODO: store dates in UTC
            surveyToUpdate.Text = survey.Text;
            surveyToUpdate.PortalId = survey.PortalId;
            surveyToUpdate.ModuleId = survey.ModuleId;
            surveyToUpdate.ShowText = true;

            surveyToUpdate.StartDate = survey.StartDate;
            surveyToUpdate.PreStartMessage = survey.PreStartMessage;
            surveyToUpdate.EndDate = survey.EndDate;
            surveyToUpdate.PostEndMessage = survey.PostEndMessage;
            
            surveyToUpdate.SendNotification = survey.SendNotification;
            surveyToUpdate.NotificationFromEmailAddress = survey.NotificationFromEmailAddress;
            surveyToUpdate.NotificationToEmailAddresses = survey.NotificationToEmailAddresses;
            surveyToUpdate.SendThankYou = survey.SendThankYou;
            surveyToUpdate.ThankYouFromEmailAddress = survey.ThankYouFromEmailAddress;
            
            surveyToUpdate.FinalMessageOption = survey.FinalMessageOption;
            surveyToUpdate.FinalMessage = "<p>" + survey.FinalMessage.Replace("\n", "<br />") + "</p>";
            surveyToUpdate.FinalUrl = survey.FinalUrl;

            surveyToUpdate.Sections.First().Text = survey.Sections.First().Text;
            surveyToUpdate.Sections.First().ShowText = true;

            surveyRepository.SubmitChanges();

            return surveyToUpdate.SurveyId;
        }

        /// <summary>
        ///   Determines whether the current request is from a user who can edit content for the given <paramref name = "moduleId" />.
        /// </summary>
        /// <param name = "moduleId">The ID of the module against which to check permissions.</param>
        /// <returns>
        ///   <c>true</c> if the current request is cleared to edit content for the given module; otherwise, <c>false</c>.
        /// </returns>
        private bool CanEditModule(int moduleId)
        {
            if (!this.Context.Items.Contains("UserInfo"))
            {
                this.SetCurrentUser();
            }

            return ModulePermissionController.CanEditModuleContent(new ModuleController().GetModule(moduleId));
        }

        /// <summary>
        ///   Denies access to any following code, by throwing a 403 exception.
        /// </summary>
        /// <exception cref = "HttpException">Always</exception>
        private void DenyAccess()
        {
            this.Context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            this.Context.Response.Flush();

            throw new HttpException((int)HttpStatusCode.Forbidden, "Could not validate user");
        }

        /// <summary>
        ///   Sets the current user so that checking authentication and roles works.
        /// </summary>
        /// <remarks>
        ///   Copies functionality from <c>DotNetNuke.HttpModules.Membership.MembershipModule.OnAuthenticateRequest</c>
        ///   to get the current user set as the "Current User"
        /// </remarks>
        private void SetCurrentUser()
        {
            // Obtain PortalSettings from Current Context
            var portalSettings = PortalController.GetCurrentPortalSettings();

            if (this.Context.Request.IsAuthenticated && portalSettings != null)
            {
                var roleController = new RoleController();
                var cachedUser = UserController.GetCachedUser(portalSettings.PortalId, this.Context.User.Identity.Name);

                if (this.Context.Request.Cookies["portalaliasid"] != null)
                {
// ReSharper disable PossibleNullReferenceException
                    var portalCookie = FormsAuthentication.Decrypt(this.Context.Request.Cookies["portalaliasid"].Value);

                    // check if user has switched portals
                    if (portalSettings.PortalAlias.PortalAliasID != int.Parse(portalCookie.UserData))
                    {
                        // expire cookies if portal has changed
                        this.Context.Response.Cookies["portalaliasid"].Value = null;
                        this.Context.Response.Cookies["portalaliasid"].Path = "/";
                        this.Context.Response.Cookies["portalaliasid"].Expires = DateTime.Now.AddYears(-30);

                        this.Context.Response.Cookies["portalroles"].Value = null;
                        this.Context.Response.Cookies["portalroles"].Path = "/";
                        this.Context.Response.Cookies["portalroles"].Expires = DateTime.Now.AddYears(-30);

// ReSharper restore PossibleNullReferenceException
                    }
                }

                // authenticate user and set last login ( this is necessary for users who have a permanent Auth cookie set ) 
                if (cachedUser == null || cachedUser.IsDeleted || cachedUser.Membership.LockedOut ||
                    cachedUser.Membership.Approved == false ||
                    cachedUser.Username.ToLower() != this.Context.User.Identity.Name.ToLower())
                {
                    var portalSecurity = new PortalSecurity();
                    portalSecurity.SignOut();

                    // Remove user from cache
                    if (cachedUser != null)
                    {
                        DataCache.ClearUserCache(portalSettings.PortalId, this.Context.User.Identity.Name);
                    }

                    // Redirect browser back to home page
                    this.Context.Response.Redirect(this.Context.Request.RawUrl, true);
                    return;
                }

                // valid Auth cookie
                // if users LastActivityDate is outside of the UsersOnlineTimeWindow then record user activity
                if (
                    DateTime.Compare(
                        cachedUser.Membership.LastActivityDate.AddMinutes(Host.UsersOnlineTimeWindow), DateTime.Now) < 0)
                {
                    // update LastActivityDate and IP Address for user
                    cachedUser.Membership.LastActivityDate = DateTime.Now;
                    cachedUser.LastIPAddress = this.Context.Request.UserHostAddress;
                    UserController.UpdateUser(portalSettings.PortalId, cachedUser);
                }

                // refreshroles is set when a role is added to a user by an administrator
                bool refreshCookies = cachedUser.RefreshRoles;

                // check for RSVP code
                if (!cachedUser.RefreshRoles && this.Context.Request.QueryString["rsvp"] != null &&
                    string.IsNullOrEmpty(this.Context.Request.QueryString["rsvp"]) == false)
                {
                    foreach (RoleInfo objRole in roleController.GetPortalRoles(portalSettings.PortalId))
                    {
                        if (objRole.RSVPCode == this.Context.Request.QueryString["rsvp"])
                        {
                            roleController.UpdateUserRole(portalSettings.PortalId, cachedUser.UserID, objRole.RoleID);

                            // clear portalroles so the new role is added to the cookie below
                            refreshCookies = true;
                        }
                    }
                }

                // create cookies if they do not exist yet for this session.
                if (this.Context.Request.Cookies["portalroles"] == null || refreshCookies)
                {
                    // keep cookies in sync
                    var currentDateTime = DateTime.Now;

                    // create a cookie authentication ticket ( version, user name, issue time, expires every hour, don't persist cookie, roles )
                    var portalTicket = new FormsAuthenticationTicket(
                        1, 
                        this.Context.User.Identity.Name, 
                        currentDateTime, 
                        currentDateTime.AddHours(1), 
                        false, 
                        portalSettings.PortalAlias.PortalAliasID.ToString());

                    // encrypt the ticket
                    string portalAliasId = FormsAuthentication.Encrypt(portalTicket);

// ReSharper disable PossibleNullReferenceException
                    // send portal cookie to client
                    this.Context.Response.Cookies["portalaliasid"].Value = portalAliasId;
                    this.Context.Response.Cookies["portalaliasid"].Path = "/";
                    this.Context.Response.Cookies["portalaliasid"].Expires = currentDateTime.AddMinutes(1);

// ReSharper restore PossibleNullReferenceException
                    // get roles from UserRoles table
                    string[] arrPortalRoles = roleController.GetRolesByUser(cachedUser.UserID, portalSettings.PortalId);

                    // create a string to persist the roles, attach a portalID so that cross-portal impersonation cannot occur
                    string strPortalRoles = portalSettings.PortalId + "!!" + string.Join(";", arrPortalRoles);

                    // create a cookie authentication ticket ( version, user name, issue time, expires every hour, don't persist cookie, roles )
                    var rolesTicket = new FormsAuthenticationTicket(
                        1, 
                        this.Context.User.Identity.Name, 
                        currentDateTime, 
                        currentDateTime.AddHours(1), 
                        false, 
                        strPortalRoles);

                    // encrypt the ticket
                    string strRoles = FormsAuthentication.Encrypt(rolesTicket);

// ReSharper disable PossibleNullReferenceException
                    // send roles cookie to client
                    this.Context.Response.Cookies["portalroles"].Value = strRoles;
                    this.Context.Response.Cookies["portalroles"].Path = "/";
                    this.Context.Response.Cookies["portalroles"].Expires = currentDateTime.AddMinutes(1);

                    if (refreshCookies)
                    {
                        // if rsvp, update portalroles in context because it is being used later
                        this.Context.Request.Cookies["portalroles"].Value = strRoles;
                    }
                }

                if (this.Context.Request.Cookies["portalroles"] != null)
                {
                    // get roles from roles cookie
                    if (this.Context.Request.Cookies["portalroles"].Value != string.Empty)
                    {
                        var roleTicket = FormsAuthentication.Decrypt(this.Context.Request.Cookies["portalroles"].Value);

// ReSharper restore PossibleNullReferenceException
                        if (roleTicket != null)
                        {
                            // get the role data and split it into portalid and a string array of role data
                            string rolesdata = roleTicket.UserData;
                            char[] separator = "!!".ToCharArray();

                            // need to use StringSplitOptions.None to preserve case where superuser has no roles
                            string[] rolesParts = rolesdata.Split(separator, StringSplitOptions.None);

                            // if cookie is for a different portal than current force a refresh of roles else used cookie cached version
                            if (Convert.ToInt32(rolesParts[0]) != portalSettings.PortalId)
                            {
                                cachedUser.Roles = roleController.GetRolesByUser(cachedUser.UserID, portalSettings.PortalId);
                            }
                            else
                            {
                                cachedUser.Roles = rolesParts[2].Split(';');
                            }
                        }
                        else
                        {
                            cachedUser.Roles = roleController.GetRolesByUser(cachedUser.UserID, portalSettings.PortalId);
                        }

                        // Clear RefreshRoles flag
                        if (cachedUser.RefreshRoles)
                        {
                            cachedUser.RefreshRoles = false;
                            UserController.UpdateUser(portalSettings.PortalId, cachedUser);
                        }
                    }

                    // save userinfo object in context
                    this.Context.Items.Add("UserInfo", cachedUser);

                    // load the personalization object
                    var personalizationController = new PersonalizationController();
                    personalizationController.LoadProfile(this.Context, cachedUser.UserID, cachedUser.PortalID);

                    // Localization.SetLanguage also updates the user profile, so this needs to go after the profile is loaded
                    Localization.SetLanguage(cachedUser.Profile.PreferredLocale);
                }
            }

            if (HttpContext.Current.Items["UserInfo"] == null)
            {
                this.Context.Items.Add("UserInfo", new UserInfo());
            }
        }
    }
}