using System;
using System.Collections.Generic;
using System.Text;

namespace RCIM_OP10
{
    class InspectionParameters
    {
        public enum Indicator
        {
            Vision=1,
            Position=2
        }

        public enum Type
        {
            Housing=1,
            PCB=2,
            Cover=3,        
            Putty=4,
            Screw=5,
            Vision=6
        }

        public enum InspectionResult
        {
            Area=1,
            Presence=2,
            Data=3, 
        }

        public enum ScrewPlane
        {
            Tridimensional = 1,
            Bidimensional = 2,
            Unidimensional = 3,
            
        }

        public struct Screwstruct
        {
            public int ScrewProg;
            public double Limit;
            public double PosX;
            public double PosY;
            public double PosZ;
        }

        public struct VisionStruct
        {
            public InspectionResult inspectionresult;
            public double MinValue;
            public double MaxValue;
            public string Ref2Compare;
            public string Description;

        }

        public string VisionProg;
        public int NumParts2check=0;
        public ScrewPlane Plane;
        public Indicator indicator =new Indicator();
        public Type type =new Type();
        //public InspectionResult inspectionresult = new InspectionResult();
        public List<Screwstruct> ScrewTable=new List<Screwstruct>();
        public List<VisionStruct> VisionTable = new List<VisionStruct>();

        public InspectionParameters() { }

        public InspectionParameters(Indicator ind, Type typ)
        {
           // inspectionresult = inspection;
            type = typ;
            indicator = ind;
        }
        public InspectionParameters(Indicator ind, Type typ, ScrewPlane plane)
        {
            indicator = ind;
            type = typ;
            Plane = plane;
        }
       
    }
}
