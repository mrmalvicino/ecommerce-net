﻿using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer;
using DomainModelLayer;

namespace BusinessLogicLayer
{
    public class RolesManager
    {
        private DataAccess _dataAccess = new DataAccess();

        public enum Ids
        {
            DefaultRoleId = 2,
            AdminRoleId = 1,
            CustomerRoleId = 2,
            DeliveryDriverRoleId = 3,
            CustomerServiceRoleId = 4,
            PlusRoleId = 5
        }

        public List<Role> List(int userId = 0)
        {
            List<Role> userRoles = new List<Role>();

            try
            {
                _dataAccess.SetProcedure("SP_List_Roles");
                _dataAccess.SetParameter("@UserId", userId);
                _dataAccess.ExecuteRead();

                while (_dataAccess.Reader.Read())
                {
                    Role role = new Role();

                    role.Id = Convert.ToInt32(_dataAccess.Reader["RoleId"]);
                    role.Name = _dataAccess.Reader["RoleName"].ToString();

                    userRoles.Add(role);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dataAccess.CloseConnection();
            }

            return userRoles;
        }

        public Role Read(int roleId)
        {
            Role role = new Role();

            try
            {
                _dataAccess.SetQuery("select RoleName from Roles where RoleId = @RoleId");
                _dataAccess.SetParameter("@RoleId", roleId);
                _dataAccess.ExecuteRead();

                if (_dataAccess.Reader.Read())
                {
                    role.Id = roleId;
                    role.Name = _dataAccess.Reader["RoleName"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dataAccess.CloseConnection();
            }

            return role;
        }

        public int Add(Role role)
        {
            // hack : No es necesario para un MVP
            return -1;
        }

        public void Edit(Role role)
        {
            // hack : No es necesario para un MVP
        }

        public int GetId(Role role)
        {
            int roleId = 0;

            try
            {
                _dataAccess.SetQuery("select RoleId from Roles where RoleName = @RoleName");
                _dataAccess.SetParameter("@RoleName", role.Name);
                _dataAccess.ExecuteRead();

                if (_dataAccess.Reader.Read())
                {
                    roleId = Convert.ToInt32(_dataAccess.Reader["RoleId"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dataAccess.CloseConnection();
            }

            return roleId;
        }

        public Role DefaultRole()
        {
            return Read((int)Ids.DefaultRoleId);
        }

        public void HandleRolesId(List<Role> roles)
        {
            if (roles == null)
            {
                roles = new List<Role>();
                roles.Add(DefaultRole());
                return;
            }

            if (roles.Count == 0)
            {
                roles.Add(DefaultRole());
            }

            for (int i = 0; i < roles.Count; i++)
            {
                HandleRoleId(roles[i]);
            }
        }

        public void HandleRoleId(Role role)
        {
            int foundId = GetId(role);

            if (foundId == 0)
            {
                role.Id = Add(role);
            }
            else if (foundId == role.Id)
            {
                Edit(role);
            }
            else
            {
                role.Id = foundId;
            }
        }

        public void UpdateRelations(User user)
        {
            List<Role> oldRoles = List(user.UserId);
            List<Role> newRoles = user.Roles;

            List<Role> rolesToRemove = oldRoles.Except(newRoles).ToList();

            foreach (Role role in rolesToRemove)
            {
                DeleteRelation(user.UserId, role.Id);
                oldRoles.Remove(role);
            }

            if (oldRoles.Count == newRoles.Count)
            {
                return;
            }

            List<Role> rolesToAdd = newRoles.Except(oldRoles).ToList();

            foreach (Role role in rolesToAdd)
            {
                AddRelation(user.UserId, role.Id);
            }
        }

        public void AddRelation(int userId, int roleId)
        {
            try
            {
                _dataAccess.SetQuery("insert into UserRoles (UserId, RoleId) values (@UserId, @RoleId)");
                _dataAccess.SetParameter("@UserId", userId);
                _dataAccess.SetParameter("@RoleId", roleId);
                _dataAccess.ExecuteAction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dataAccess.CloseConnection();
            }
        }

        public void DeleteRelation(int userId, int roleId)
        {
            try
            {
                _dataAccess.SetQuery("delete from UserRoles where UserId = @UserId and RoleId = @RoleId");
                _dataAccess.SetParameter("@UserId", userId);
                _dataAccess.SetParameter("@RoleId", roleId);
                _dataAccess.ExecuteAction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dataAccess.CloseConnection();
            }
        }
    }
}
