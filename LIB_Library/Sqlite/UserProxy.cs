﻿using Core.Helpers;
using Core.Interfaces.DbBrowser;
using LIB.Constants;
using LIB.Entities.Data.Base;
using LIB.Entities.Data.Tris;
using LIB.Sqlite.Base;
using LIB.Sqlite.Entities;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Sqlite
{
    public class UserProxy : ProxyBase
    {
        public UserProxy(string UserName) 
            : base(String.Format(Cnst.UserGameDataLocation, UserName))
        {
        }

        public override string GetCreateTableIfExistsStatement()
        {
            return $@"CREATE TABLE IF NOT EXISTS TRIS_SP
                        (
	                        {DataManagment.GAME_DATA_TABLE_COMMON},
	                        HAS_WON BOOL NOT NULL
                        );
                        CREATE TABLE IF NOT EXISTS TRIS_MP
                        (
	                        {DataManagment.GAME_DATA_TABLE_COMMON},
	                        HAS_WON BOOL NOT NULL,
	                        OPPONENT_NAME VARCHAR(255) NULL
                        );";
        }

        public bool SaveData(object data)
        {
            bool result = false;
            switch (data)
            {
                case GameDataTrisSp gameDataTrisSp:
                    result = SaveTrisSp(data as GameDataTrisSp);
                    break;
                case GameDataTrisMp gameDataTrisMp:
                    break;
                default:
                    MessageDialogHelper.ShowInfoMessage($"Il tipo passato non è gestito nel metodo {MethodInfo.GetCurrentMethod().Name}, ({data.GetType().FullName})");
                    break;
            }
            return result;
        }

        public bool UpdateData(object data)
        {
            bool result = false;
            switch (data)
            {
                case GameDataTrisSp gameDataTrisSp:
                    result = UpdateTrisSp(data as GameDataTrisSp);
                    break;
                case GameDataTrisMp gameDataTrisMp:
                    break;
                default:
                    MessageDialogHelper.ShowInfoMessage($"Il tipo passato non è gestito nel metodo {MethodInfo.GetCurrentMethod().Name}, ({data.GetType().FullName})");
                    break;
            }


            return result;
        }
        private SQLiteParameters GetBaseParameters(GameDataBase data)
        {
            SQLiteParameters parameters = new SQLiteParameters();
            parameters.Add("@GAME_ID", data.GameId);
            parameters.Add("@GAME_GUID", data.GameGUID);
            parameters.Add("@USER_NAME", data.UserName);
            parameters.Add("@GAME_DATE", data.GameDate);
            return parameters;
        }

        #region Tris Sp
        private bool SaveTrisSp(GameDataTrisSp data)
        {
            bool result = false;
            string Statement = @"INSERT INTO TRIS_SP
           (GAME_ID
           ,GAME_GUID
           ,USER_NAME
           ,GAME_DATE
           ,HAS_WON)
     VALUES (
             @GAME_ID
            ,@GAME_GUID
            ,@USER_NAME
            ,@GAME_DATE
            ,@HAS_WON           
            )";

            data.GameId = GetNextIntValue("TRIS_SP", "GAME_ID");
            SQLiteParameters parameters = GetBaseParameters(data);
            parameters.Add("@HAS_WON", data.UserHasWon);

            result = Execute(Statement, parameters);
            return result;
        }
        private bool UpdateTrisSp(GameDataTrisSp data)
        {
            bool result = false;
            string Statement = @"UPDATE TRIS_SP SET HAS_WON = @HAS_WON
                         WHERE GAME_ID = @GAME_ID;
                        ";
            SQLiteParameters parameters = new SQLiteParameters();
            parameters.Add("@GAME_ID", data.GameId);
            parameters.Add("@HAS_WON", data.UserHasWon);
            result = Execute(Statement, parameters);
            return result;
        }
        #endregion
    }
}
