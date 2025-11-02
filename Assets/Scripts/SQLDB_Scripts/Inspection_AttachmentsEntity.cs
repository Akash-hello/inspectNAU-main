using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBank
{
    public class Inspection_AttachmentsEntity
    {
        //Such “Entity” classes are used for easy data handling. It’s not like everyone will remember the order of columns in their tables.
        public int _ID;
        public int _Inspection_PrimaryDetails_ID;
        public int _Inspection_Observations_ID;
        public string _Attachment_Title;
        public string _Attachment_Path;
        public string _Attachment_Name;
        public string _Attachment_Details_1;
        public string _Attachment_Details_2;
        public string _Attachment_Details_3;
        public string _Attachment_Details_4;
        public string _Active;
        public string _Timestamp;

        public Inspection_AttachmentsEntity(int ID, int Inspection_PrimaryDetails_ID, int Inspection_Observations_ID, string Attachment_Title, string Attachment_Name, string Attachment_Path, string Attachment_Details_1, string Attachment_Details_2, string Attachment_Details_3, string Attachment_Details_4, string Active, string Timestamp)
        {
            _ID = ID;
            _Inspection_PrimaryDetails_ID = Inspection_PrimaryDetails_ID;
            _Inspection_Observations_ID = Inspection_Observations_ID;
            _Attachment_Title = Attachment_Title;
            _Attachment_Name = Attachment_Name;
            _Attachment_Path = Attachment_Path;
            _Attachment_Details_1 = Attachment_Details_1;
            _Attachment_Details_2 = Attachment_Details_2;
            _Attachment_Details_3 = Attachment_Details_3;
            _Attachment_Details_4 = Attachment_Details_4;
            _Active = Active;
            _Timestamp = Timestamp;
        }

        public static Inspection_AttachmentsEntity getFakeAttachmentDetails()
        {
            return new Inspection_AttachmentsEntity(0, 1, 2, "Attachment Title", "Attachment Name", "Attachment Path", "Attachment Details 1", "Attachment Details 2", "Attachment Details 3", "Attachment Details 4", "Y", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"));
        }
    }

}
