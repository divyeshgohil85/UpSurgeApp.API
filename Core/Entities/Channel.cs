using Core.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    //public class ChannelValues
    //{
    //    [Key]
    //    public int Id { get; set; }

    //    public string ChannelName { get; set; }

    //    public long TimeToke { get; set; }
    //}

    public class Channel
    {
        [Key]
        public int Id { get; set; }

        public string ChannelName { get; set; }

        public long TimeToke { get; set; }

        //[Column(TypeName = "nvarchar(max)")]
        //public ChannelValues Values { get; set; }
    }
}
