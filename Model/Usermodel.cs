﻿namespace keyclock_Authentication.Model
{
    public class TblpreferanceClass
    {
           public int id { get; set; }

            public string name { get; set; }

            public int sub_id { get; set; }
       
    }
    public class TblSubPreferance
    {
        public int id { get; set; }

        public string name { get; set; }

        public int pref_id { get; set; }

    }

    public class cmsclass
    {
        public int id { get; set; }
        public string title { get; set; }

        public string description { get; set; }

        public string image { get; set; } //Iformfile

        public int subId { get; set; }

        public int subPreferenceId { get; set; }

        public string prefname { get; set; }

    }
}
