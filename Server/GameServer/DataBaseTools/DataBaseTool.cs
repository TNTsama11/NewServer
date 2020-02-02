using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Threading;
using ServerOne;
using CommunicationProtocol.Code;
using CommunicationProtocol.Dto;
using GameServer.Cache;
using System.Data;

namespace GameServer.DataBaseTools
{
    public class DataBaseTool
    {
        private string connstr;
        public DataBaseTool(string dataSource,string database,string userID,string password)
        {
            connstr = "data source=" + dataSource + ";" + "database=" + database + ";" + "user id =" + userID + ";" + "password=" + password + ";"+ "charset=utf8";
        }
        /// <summary>
        /// 在数据库中查询是否存在该记录并且发给客户端
        /// </summary>
        public void UserIsExist(string acc,ClientPeer client)
        {
            QueryArgument arg = new QueryArgument();
            arg.Account = acc;
            arg.client = client;
            Thread t = new Thread(QueryAcc);
            t.Start(arg);           
        }
        private void QueryAcc(object arg)
        {
            QueryArgument argument=arg as QueryArgument;
            if (argument == null)
            {
                return;
            }
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connstr))
                {
                    string SQL = "SELECT * FROM users WHERE account=@acc";
                    DataSet ds = new DataSet();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);
                    cmd.Parameters.Add(new MySqlParameter("@acc",argument.Account));
                    conn.Open();
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
                    dataAdapter.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string account = argument.Account;
                        string name = ds.Tables[0].Rows[0][2].ToString();
                        int iconId = (int)ds.Tables[0].Rows[0][4];
                        int modelId = (int)ds.Tables[0].Rows[0][5];
                        int lv = (int)ds.Tables[0].Rows[0][3];
                        UserDto dto = new UserDto(account,name,iconId,modelId,lv);
                        argument.client.SendMessage(OpCode.ACCOUNT, AccCode.ACC_LOGIN_SREP, dto);
                        Caches.acc.Onlie(argument.client, argument.Account);
                    }
                    else
                    {
                        argument.client.SendMessage(OpCode.ACCOUNT, AccCode.ACC_LOGIN_NOACC,null);
                    }
                }
            }
            catch (Exception ex)
            {
                Tool.PrintMessage(ex.ToString());
            }
        }
        /// <summary>
        /// 创建一个新记录并发给客户端
        /// </summary>
        public void Create(string acc,ClientPeer client)
        {
            QueryArgument arg = new QueryArgument();
            arg.Account = acc;
            arg.client = client;
            Thread t = new Thread(Insert);
            t.Start(arg);
        }

        private void Insert(object arg)
        {
            QueryArgument argument = arg as QueryArgument;
            if (argument == null)
            {
                return;
            }
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connstr))
                {
                    string SQL = "INSERT INTO users (account,user_name,user_lv,icon_id,model_id) VALUES (@acc,@name,@lv,@icon,@model)";
                    UserDto dto = new UserDto(argument.Account, "NameLess", 0, 0, 1);
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);
                    cmd.Parameters.Add(new MySqlParameter("@acc",dto.Account));
                    cmd.Parameters.Add(new MySqlParameter("@name", dto.Name));
                    cmd.Parameters.Add(new MySqlParameter("@lv", dto.Lv));
                    cmd.Parameters.Add(new MySqlParameter("@icon", dto.IconID));
                    cmd.Parameters.Add(new MySqlParameter("@model", dto.ModelID));
                    conn.Open();
                   int e= cmd.ExecuteNonQuery();
                    if (e == 0)
                    {
                        Tool.PrintMessage("数据插入失败");
                        dto.Account = "0";
                        argument.client.SendMessage(OpCode.ACCOUNT, AccCode.ACC_CREATE_SREP, dto);
                    }
                    else
                    {
                        Tool.PrintMessage("数据插入成功");                       
                        argument.client.SendMessage(OpCode.ACCOUNT, AccCode.ACC_CREATE_SREP, dto);
                    }
                }
            }
            catch(Exception ex)
            {
                Tool.PrintMessage(ex.ToString());
            }
        }

        public void UserReload(string acc, ClientPeer client)
        {
            QueryArgument arg = new QueryArgument();
            arg.Account = acc;
            arg.client = client;
            Thread t = new Thread(Reload);
            t.Start(arg);
        }
        private void Reload(object arg)
        {
            QueryArgument argument = arg as QueryArgument;
            if (argument == null)
            {
                return;
            }
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connstr))
                {
                    string SQL = "SELECT * FROM users WHERE account=@acc";
                    DataSet ds = new DataSet();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);
                    cmd.Parameters.Add(new MySqlParameter("@acc", argument.Account));
                    conn.Open();
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
                    dataAdapter.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string account = argument.Account;
                        string name = ds.Tables[0].Rows[0][2].ToString();
                        int iconId = (int)ds.Tables[0].Rows[0][4];
                        int modelId = (int)ds.Tables[0].Rows[0][5];
                        int lv = (int)ds.Tables[0].Rows[0][3];
                        UserDto dto = new UserDto(account, name, iconId, modelId, lv);
                        argument.client.SendMessage(OpCode.ACCOUNT, AccCode.ACC_RELOAD_SREP, dto);
                        Caches.acc.Onlie(argument.client, argument.Account);
                    }
                    else
                    {
                        argument.client.SendMessage(OpCode.ACCOUNT, AccCode.ACC_RELOAD_SREP, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Tool.PrintMessage(ex.ToString());
            }
        }

        public void UpdataUser(ClientPeer client,string acc,UserDto dto)
        {
            UpdataUserArg userArg = new UpdataUserArg();
            userArg.Client = client;
            userArg.Account = acc;
            userArg.Dto = dto;
            Thread t = new Thread(Updata);
            t.Start(userArg);
        }

        private void Updata(object arg)
        {
            UpdataUserArg userArg = arg as UpdataUserArg;
            if (arg == null)
            {
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connstr))
                {
                    string SQL = "UPDATE users SET user_name=@name,user_lv=@lv,icon_id=@icon,model_id=@model WHERE account=@acc";
                    UserDto userDto = userArg.Dto;
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);
                    cmd.Parameters.Add(new MySqlParameter("@name", userDto.Name));
                    cmd.Parameters.Add(new MySqlParameter("@lv", userDto.Lv));
                    cmd.Parameters.Add(new MySqlParameter("@icon", userDto.IconID));
                    cmd.Parameters.Add(new MySqlParameter("@model", userDto.ModelID));
                    cmd.Parameters.Add(new MySqlParameter("@acc", userArg.Account));
                    conn.Open();
                    int e = cmd.ExecuteNonQuery();
                    if (e == 0)
                    {
                        Tool.PrintMessage("数据修改失败");
                    }
                    else
                    {
                        Tool.PrintMessage("数据修改成功");
                    }
                }
            }
            catch(Exception ex)
            {
                Tool.PrintMessage(ex.ToString());
            }
        }
    }
}
