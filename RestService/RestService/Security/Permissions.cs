using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using RestService.Entities;

namespace RestService.Security
{
    public class Permissions
    {
        public static Permissions createPermissions(User user, DatabaseConnection db)
        {
            PreparedStatement preStmtUser = db.Prepare("SELECT action.name, content_id, allow FROM user_account_can_do_action, action WHERE action.id = user_account_can_do_action.action_id AND user_account_id = "+user.id, new List<string>());
            PreparedStatement preStmtGroup = db.Prepare("SELECT action.name, content_id, allow FROM action, user_group_can_do_action, (SELECT user_group_id FROM user_account_in_user_group WHERE user_account_id = " + user.id + ") userGroups WHERE action.id = user_group_can_do_action.action_id AND user_group_can_do_action.user_group_id = userGroups.user_group_id", new List<string>());

            List<RestService.Entities.Action> actions = new List<RestService.Entities.Action>();

            Console.WriteLine(preStmtUser.GetCmd().CommandText);

            SqlDataReader reader = db.Query(new Dictionary<string,string>(),preStmtUser);

            while (reader.Read())
            {
                int contentId = int.Parse(reader.GetString(reader.GetOrdinal("content_id")));
                string actionName = reader.GetString(reader.GetOrdinal("name"));
                //bool allowed = reader.GetBoolean(reader.GetOrdinal("allow"));

                actions.Add(new Entities.Action(contentId,actionName,null, true));
            }

            reader = db.Query(new Dictionary<string,string>(), preStmtGroup);

            while (reader.Read())
            {
                int contentId = reader.GetInt32(reader.GetOrdinal("content_id"));
                string actionName = reader.GetString(reader.GetOrdinal("name"));
                //bool allowed = reader.GetBoolean(reader.GetOrdinal("allowed"));

                actions.Add(new Entities.Action(contentId, actionName, null, true));
            }

            return new Permissions(actions.ToArray(), user);
        }

        public RestService.Entities.Action[] actions;
        public User user;

        private Permissions(RestService.Entities.Action[] actions, User user)
        {
            this.actions = actions;
            this.user = user;
        }

        public bool canDo(RestService.Entities.Action action)
        {
            bool allowed = false;

            for (int i = 0; i < actions.Length; i++)
            {
                if (action.Equals(actions[i]))
                {
                    if (actions[i].allowed)
                    {
                        allowed = true;
                    }
                    else if (!actions[i].allowed)
                    {
                        return false;
                    }
                }
            }

            return allowed;
        }
    }
}