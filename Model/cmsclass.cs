﻿namespace cmsApi
{
    public class cmsclass
    {

        public int? id { get; set; }
        public string? title { get; set; }

        public string? description { get; set; } 

        public  string image { get; set; }

        public int prefId { get; set; }

        public int subPreferenceId { get; set; }
    
     public bool approved{ get; set; }
    }

    public class feedbackdata
    {

        public string name { get; set; }

        public string email { get; set; }

        public string phoneno { get; set; }

        public string feedback { get; set; }
    }
}



