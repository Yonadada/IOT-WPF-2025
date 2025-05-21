using System.ComponentModel.DataAnnotations;

namespace WpfMqttSubApp.Models
{
    public class FakeInfo
    {
        [Key] //어노테이션으로 키 설정
        public DateTime Sensing_Dt { get; set; }

        [Key]
        public string Pub_Id { get; set; }

        public decimal Count { get; set; }

        public float Temp { get; set; }

        public float Humid { get; set; }

        public bool Light { get; set; }

        public bool Human { get; set; }


    }
}
