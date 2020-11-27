using System;


namespace RCIM_OP10
{
    struct MoveIn_Result
    {
        public bool Response;
        public string Error;
    }
    class Cim
    {
        MES_HAI.Traceability CIM = new MES_HAI.Traceability();
        
        public bool Login(string Station)
        {
            bool Connect = (CIM.Login(Station) == MES_HAI.Entity.Enums.ConnectionState.Logged) ? true : false;
            return Connect;
            
        }
        public bool Disconnect()
        {
            return CIM.Disconnect();
        }
        public bool CheckConnection()
        {
            return CIM.Connection_Ping();
        }
        private string Error_Process(string Menssage)
        {
            string Error = "";
            if (Menssage != "")
            {
                char[] ArrayError = Menssage.ToCharArray();
                if (ArrayError != null)
                {
                    for (int index = 0; index < ArrayError.Length; ++index)
                    {
                        if (Convert.ToString(ArrayError[index]) == Convert.ToString(ArrayError[index]).ToUpper() && index > 0)
                        {
                            Error += " " + ArrayError[index];
                        }
                        else
                        {
                            Error += ArrayError[index];
                        }
                    }
                }
            }
            return Error;
        }
        public  MoveIn_Result MoveIn(string Station, string Serial,ref string ResponseResult)
        {
            MoveIn_Result Result = new MoveIn_Result();
            Result.Response = false;
            Result.Error = "";
            var Response = CIM.Serial_MoveIn(Station, Serial, true, 0);
            if (Response.Result == MES_HAI.Entity.Enums.Results.Pass)
            {
                
                if (Response.Station == Station)
                {
                    if (Response.UnitId == Serial)
                    {
                        Result.Response = true;
                    }
                    else
                    {
                        Result.Response = false;
                        Result.Error = "Serial Does Not Match";
                    }
                }
                else
                {
                    Result.Response = false;
                    Result.Error = "Station Does Not Match";
                }
            }
            else
            {
                if (Response.Station == Station)
                {
                    if (Response.UnitId == Serial)
                    {
                        Result.Error = Error_Process(Response.ErrorDescription);
                    }
                    else
                    {
                        Result.Response = false;
                        Result.Error = "Serial Does Not Match";
                    }
                }
                else
                {
                    Result.Response = false;
                    Result.Error = "Station Does Not Match";
                }

            }
            ResponseResult = Response.ErrorDescription;
            return Result;
        }


        public MoveIn_Result MoveOut(string Station, string Serial, bool pass)
        {
            MoveIn_Result Result_Send = new MoveIn_Result();
            Result_Send.Response = true;
            Result_Send.Error = "";
            MES_HAI.Entity.Enums.Results SerialResult = new MES_HAI.Entity.Enums.Results();
            if (pass)
            {
                SerialResult = MES_HAI.Entity.Enums.Results.Pass;
            }
            else
            {
                SerialResult = MES_HAI.Entity.Enums.Results.Fail;
            }
            var Errors = CIM.Serial_MoveOut(Station, Serial, SerialResult, 0, false);
            Result_Send.Error = Error_Process(Errors.ErrorDescription);
            return Result_Send;
        }

        public MoveIn_Result GetInformation(string Station, string Serial)
        {
            MoveIn_Result Result = new MoveIn_Result();
            Result.Response = false;
            Result.Error = "";
            var Response = CIM.Serial_GetInformation(Station, Serial);
            
            Result.Response = true;
            return Result;
        }

        public MoveIn_Result Built(string Station, string Children, string Serial)
        {
            MoveIn_Result Result = new MoveIn_Result();
            Result.Response = false;
            Result.Error = "";
            var Response = CIM.Serial_Correlation(Station, Children, Serial);

            Result.Response = true;
            return Result;
        }
        public MoveIn_Result MoveOut(string Station, string Serial, bool pass, string Notes)
        {
            
            MoveIn_Result Result_Send = new MoveIn_Result();
            Result_Send.Response = true;
            Result_Send.Error = "";
            MES_HAI.Entity.Enums.Results SerialResult = new MES_HAI.Entity.Enums.Results();
            MES_HAI.Entity.Measure[] Results = new MES_HAI.Entity.Measure[1];
            MES_HAI.Entity.Measure Result = new MES_HAI.Entity.Measure();

            if (pass)
            {
                SerialResult = MES_HAI.Entity.Enums.Results.Pass;
                Result.MeasureValue = "1";
                Result.Result = MES_HAI.Entity.Enums.MeasureResults.Pass;
            }
            else
            {
                SerialResult = MES_HAI.Entity.Enums.Results.Fail;
                Result.MeasureValue = "0";
                Result.Result = MES_HAI.Entity.Enums.MeasureResults.Fail;
            }
            //New            
            Result.HighLimit = "1";
            Result.LowLimit = "0";
            Result.MeasureKey = "Pass/Fail";
            Result.MeasureNotes = Notes;
            Result.Position = 0;
            Results[0] = Result;
            var Errors = CIM.Serial_MoveOutAndTestResults(Station, Serial, SerialResult, "1", "1.1", Results, 0, false);
            Result_Send.Error = Error_Process(Errors.ErrorDescription);
            return Result_Send;
        }
    }
}
