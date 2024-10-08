﻿using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using DomainModelLayer;

namespace WebForms.Admin
{
    public partial class Users : System.Web.UI.Page
    {
        // ATTRIBUTES

        private User _user;
        private User _sessionUser;
        private Role _role;
        private List<User> _users;
        private UsersManager _usersManager;
        private RolesManager _rolesManager;

        // CONSTRUCT

        public Users()
        {
            _usersManager = new UsersManager();
            _rolesManager = new RolesManager();
            FetchUsers();
        }

        // METHODS

        private void FetchSessionUser()
        {
            _sessionUser = (User)Session["user"];
        }

        private void FetchUsers()
        {
            _users = _usersManager.List();
        }

        private void BindUsersRpt()
        {
            UsersRpt.DataSource = _users;
            UsersRpt.DataBind();
        }

        private void CheckUserPermissions()
        {
            if (_sessionUser == null)
            {
                Response.Redirect("/AccessDenied.aspx");
                return;
            }

            else if (!_usersManager.IsAdmin(_sessionUser))
            {
                Response.Redirect("/AccessDenied.aspx");
                return;
            }
        }

        private void UpdateButtonClass(LinkButton btn)
        {
            string cssClass = "";

            if (_role.Id == (int)RolesManager.Ids.AdminRoleId)
            {
                cssClass += "bi bi-person-badge";
            }
            else if (_role.Id == (int)RolesManager.Ids.CustomerRoleId)
            {
                cssClass += "bi bi-cart";
            }
            else if (_role.Id == (int)RolesManager.Ids.DeliveryDriverRoleId)
            {
                cssClass += "bi bi-truck";
            }
            else if (_role.Id == (int)RolesManager.Ids.CustomerServiceRoleId)
            {
                cssClass += "bi bi-headset";
            }
            else if (_role.Id == (int)RolesManager.Ids.PlusRoleId)
            {
                cssClass += "bi bi-star";
            }

            if (_usersManager.UserHasRole(_user, _role))
            {
                cssClass += " btn btn-outline-primary";
            }
            else
            {
                cssClass += " btn btn-outline-secondary";
            }

            btn.CssClass = cssClass;
        }

        // EVENTS

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FetchSessionUser();
                CheckUserPermissions();
                FetchUsers();
                BindUsersRpt();
            }
        }

        protected void UsersRpt_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                _user = (User)e.Item.DataItem;

                Repeater rpt = (Repeater)e.Item.FindControl("RolesRpt");
                rpt.DataSource = _rolesManager.List();
                rpt.DataBind();

                int itemIndex = e.Item.ItemIndex;

                foreach (RepeaterItem roleItem in rpt.Items)
                {
                    LinkButton roleBtn = (LinkButton)roleItem.FindControl("RoleBtn");
                    if (roleBtn != null)
                    {
                        roleBtn.CommandArgument = roleBtn.CommandArgument + ";" + itemIndex;
                    }
                }
            }
        }

        protected void RolesRpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                _role = (Role)e.Item.DataItem;

                LinkButton roleBtn = (LinkButton)e.Item.FindControl("RoleBtn");

                UpdateButtonClass(roleBtn);
            }
        }

        protected void RolesRpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "RoleClick")
            {
                string[] args = e.CommandArgument.ToString().Split(';');

                int roleId = int.Parse(args[0]);
                _role = _rolesManager.Read(roleId);

                int index = int.Parse(args[1]);
                _user = _users[index];

                _usersManager.ToggleUserRole(_user, _role);

                if (_user.UserId == ((User)Session["user"]).UserId)
                {
                    Session["user"] = _usersManager.Read(_user.UserId);
                }

                FetchUsers();
                BindUsersRpt();
            }
        }

        protected void SearchBtn_Click(object sender, EventArgs e)
        {

        }
    }
}