using Core.Helpers;
using Core.Interfaces.DbBrowser;
using LIB.Constants;
using LIB.Entities.Data.Base;
using LIB.Entities.Data.Memory;
using LIB.Entities.Data.Tris;
using LIB.Sqlite.Base;
using LIB.Sqlite.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        public const string GAME_ID = "GAME_ID";
        public UserProxy(string UserName) 
            : base(String.Format(Cnst.UserGameDataLocation, UserName))
        {
        }

        public override string GetCreateTableIfExistsStatement()
        {
            return $@"CREATE TABLE IF NOT EXISTS TRIS_SP
                        (
	                        {DataManagment.GAME_DATA_TABLE_COMMON},
	                        GAME_RESULT SHORT_INT NOT NULL
                        );
                        CREATE TABLE IF NOT EXISTS TRIS_MP
                        (
	                        {DataManagment.GAME_DATA_TABLE_COMMON},
	                        GAME_RESULT SHORT_INT NOT NULL,
	                        OPPONENT_NAME VARCHAR(255) NULL
                        );
                        CREATE TABLE IF NOT EXISTS SUPER_TRIS_MP 
                        (	
	                        {DataManagment.GAME_DATA_TABLE_COMMON},
                            GAME_RESULT INT NOT NULL, 
	                        OPPONENT_NAME VARCHAR(255) NULL,
                            STARTING_TIME INT NOT NULL,
	                        REMAINING_TIME INT NOT NULL,
	                        CELLS_WON INT NOT NULL
                        );
                        CREATE TABLE IF NOT EXISTS MEMORY_SP
                        (
	                        {DataManagment.GAME_DATA_TABLE_COMMON},
	                        CARDS_NUMBER INT NOT NULL,
	                        GAME_DIFFICULTY TINYINT NOT NULL,
	                        ERRORS_LIMIT INT NOT NULL,
	                        ERRORS_NUMBER INT NOT NULL,
	                        POINTS INT NOT NULL,
	                        GAME_RESULT TINYINT NOT NULL
                        )
                        ";
        }

        public bool SaveData(object data)
        {
            bool result = false;
            switch (data)
            {
                case GameDataTrisSp gameDataTrisSp:
                    result = SaveTrisSp(gameDataTrisSp);
                    break;
                case GameDataTrisMp gameDataTrisMp:
                    result = SaveTrisMp(gameDataTrisMp);
                    break;
                case GameDataSuperTrisMp gameDataSuperTrisMp:
                    result = SaveSuperTrisMp(gameDataSuperTrisMp);
                    break;
                case GameDataMemorySp gameDataMemorySp:
                    result = SaveMemorySp(gameDataMemorySp);
                    break;
                case GameDataMemoryMp gameDataMemoryMp:
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
                    result = UpdateTrisSp(gameDataTrisSp);
                    break;
                case GameDataTrisMp gameDataTrisMp:
                    result = UpdateTrisMp(gameDataTrisMp);
                    break;
                case GameDataSuperTrisMp gameDataSuperTrisMp:
                    result = UpdateSuperTrisMp(gameDataSuperTrisMp);
                    break;
                case GameDataMemorySp gameDataMemorySp:
                    UpdateMemorySp(gameDataMemorySp);
                    break;
                case GameDataMemoryMp gameDataMemoryMp:
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
           ,GAME_RESULT)
     VALUES (
             @GAME_ID
            ,@GAME_GUID
            ,@USER_NAME
            ,@GAME_DATE
            ,@GAME_RESULT           
            )";

            data.GameId = GetNextIntValue("TRIS_SP", GAME_ID);
            SQLiteParameters parameters = GetBaseParameters(data);
            parameters.Add("@GAME_RESULT", (int)data.GameResults);

            result = Execute(Statement, parameters);
            return result;
        }
        private bool UpdateTrisSp(GameDataTrisSp data)
        {
            bool result = false;
            string Statement = @"UPDATE TRIS_SP SET GAME_RESULT = @GAME_RESULT
                         WHERE GAME_ID = @GAME_ID;
                        ";
            SQLiteParameters parameters = new SQLiteParameters();
            parameters.Add("@GAME_ID", data.GameId);
            parameters.Add("@GAME_RESULT", (int)data.GameResults);
            result = Execute(Statement, parameters);
            return result;
        }
        #endregion

        #region Tris Mp
        private bool SaveTrisMp(GameDataTrisMp data)
        {
            bool result = false;
            string Statement = @"INSERT INTO TRIS_MP
           (GAME_ID
           ,GAME_GUID
           ,USER_NAME
           ,GAME_DATE
           ,GAME_RESULT
           ,OPPONENT_NAME)
     VALUES
           (@GAME_ID
           ,@GAME_GUID
           ,@USER_NAME
           ,@GAME_DATE
           ,@GAME_RESULT
           ,@OPPONENT_NAME
);
";
            data.GameId = GetNextIntValue("TRIS_MP", GAME_ID);
            SQLiteParameters parameters = GetBaseParameters(data);
            parameters.Add("@GAME_RESULT", (int)data.GameResults);
            parameters.Add("@OPPONENT_NAME", data.OpponentName);
            result = Execute(Statement, parameters);
            return result;
        }

        private bool UpdateTrisMp(GameDataTrisMp data)
        {
            bool result = false;
            string Statement = @"UPDATE TRIS_MP SET 
                                GAME_RESULT = @GAME_RESULT
                                WHERE GAME_ID = @GAME_ID";
            SQLiteParameters parameters = new SQLiteParameters();
            parameters.Add("@GAME_RESULT", (int)data.GameResults);
            parameters.Add("@GAME_ID", data.GameId);
            result = Execute(Statement, parameters);
            return result;
        }
        #endregion

        #region SuperTrisMp
        private bool SaveSuperTrisMp(GameDataSuperTrisMp data)
        {
            bool result = false;
            string Statement = @"INSERT INTO SUPER_TRIS_MP (
                            GAME_ID
	                        ,GAME_GUID 
	                        ,USER_NAME 
	                        ,GAME_DATE 
                            ,GAME_RESULT
	                        ,OPPONENT_NAME 
                            ,STARTING_TIME
	                        ,REMAINING_TIME 
	                        ,CELLS_WON) VALUES(
                            @GAME_ID
	                        ,@GAME_GUID 
	                        ,@USER_NAME 
	                        ,@GAME_DATE 
                            ,@GAME_RESULT
	                        ,@OPPONENT_NAME 
                            ,@STARTING_TIME
	                        ,@REMAINING_TIME 
	                        ,@CELLS_WON );";
            data.GameId = GetNextIntValue("SUPER_TRIS_MP", GAME_ID);
            SQLiteParameters parameters = GetBaseParameters(data);
            parameters.Add("@GAME_RESULT", (int)data.GameResults);
            parameters.Add("@OPPONENT_NAME", data.OpponentName);
            parameters.Add("@STARTING_TIME", data.StartingTime);
            parameters.Add("@REMAINING_TIME", data.RemainingTime);
            parameters.Add("@CELLS_WON", data.CellsWon);
            result = Execute(Statement, parameters);
            return result;
        }

        private bool UpdateSuperTrisMp(GameDataSuperTrisMp data)
        {
            bool result = false;
            string Statement = @"UPDATE SUPER_TRIS_MP SET
                                GAME_RESULT = @GAME_RESULT,
                                STARTING_TIME = @STARTING_TIME,
                                REMAINING_TIME = @REMAINING_TIME,
                                CELLS_WON = @CELLS_WON
                                WHERE GAME_ID = @GAME_ID";
            SQLiteParameters parameters = new SQLiteParameters();
            parameters.Add("@GAME_RESULT", (int)data .GameResults);
            parameters.Add("@STARTING_TIME", data.StartingTime);
            parameters.Add("@REMAINING_TIME", data.RemainingTime);
            parameters.Add("@CELLS_WON", data.CellsWon);
            parameters.Add("@GAME_ID", data.GameId);
            result = Execute(Statement, parameters);
            return result;
        }
        #endregion

        #region Memory Sp
        private bool SaveMemorySp(GameDataMemorySp data)
        {
            bool result = false;
            string Statement = @"INSERT INTO MEMORY_SP
                               (GAME_ID
                               ,GAME_GUID
                               ,USER_NAME
                               ,GAME_DATE
                               ,CARDS_NUMBER
                               ,GAME_DIFFICULTY
                               ,ERRORS_LIMIT
                               ,ERRORS_NUMBER
                               ,POINTS
                               ,GAME_RESULT)
                         VALUES
                               (@GAME_ID
                               ,@GAME_GUID
                               ,@USER_NAME
                               ,@GAME_DATE
                               ,@CARDS_NUMBER
                               ,@GAME_DIFFICULTY
                               ,@ERRORS_LIMIT
                               ,@ERRORS_NUMBER
                               ,@POINTS
                               ,@GAME_RESULT)";
            data.GameId = GetNextIntValue("MEMORY_SP", GAME_ID);
            SQLiteParameters parameters = GetBaseParameters(data);
            parameters.Add("@CARDS_NUMBER", data.CardsNumber);
            parameters.Add("@GAME_DIFFICULTY", (short)data.GameDifficulty);
            parameters.Add("@ERRORS_LIMIT", data.ErrorsLimit);
            parameters.Add("@ERRORS_NUMBER", data.ErrorsNumber);
            parameters.Add("@POINTS", data.Points);
            parameters.Add("@GAME_RESULT", (short)data.GameResult);
            result = Execute(Statement, parameters);
            return result;
        }
        private bool UpdateMemorySp(GameDataMemorySp data)
        {
            bool result = false;
            string Statement = @"UPDATE MEMORY_SP 
                                 SET GAME_DATE = @GAME_DATE
                                ,CARDS_NUMBER = @CARDS_NUMBER
                                ,GAME_DIFFICULTY = @GAME_DIFFICULTY
                                ,ERRORS_LIMIT = @ERRORS_LIMIT
                                ,ERRORS_NUMBER = @ERRORS_NUMBER
                                ,POINTS = @POINTS
                                ,GAME_RESULT = @GAME_RESULT
                                 WHERE GAME_ID = @GAME_ID;
                        ";
            SQLiteParameters parameters = new SQLiteParameters();
            parameters.Add("@GAME_ID", data.GameId);
            parameters.Add("@GAME_DATE", data.GameDate);
            parameters.Add("@CARDS_NUMBER", data.CardsNumber);
            parameters.Add("@GAME_DIFFICULTY", (short)data.GameDifficulty);
            parameters.Add("@ERRORS_LIMIT", data.ErrorsLimit);
            parameters.Add("@ERRORS_NUMBER", data.ErrorsNumber);
            parameters.Add("@POINTS", data.Points);
            parameters.Add("@GAME_RESULT", (short)data.GameResult);

            result = Execute(Statement, parameters);
            return result;
        }
        #endregion
    }
}
