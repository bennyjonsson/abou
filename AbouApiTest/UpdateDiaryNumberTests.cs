using System;
using Xunit;
using AbouApi;
using AbouApi.Entities;

namespace AbouApiTest
{
    public partial class UpdateDiaryNumberTests
    {
        [Fact]
        public void UpdateDiaryNumberData()
        {
            var config = Global.GetConfig;
            var api = new AbouRestApi(config);

            var data = new DataDiaryNumber(config)
            {
                UniqueId = "190227-BSF_10-GC95",
                DiaryNumber = Guid.NewGuid().ToString()
            };
            api.UpdateDiaryNumber(data);
        }


        [Fact]
        public void UpdateDiaryNumber()
        {
            var config = Global.GetConfig;
            var api = new AbouRestApi(config);

            api.UpdateDiaryNumber("190227-BSF_10-GC95", Guid.NewGuid().ToString());
        }


        [Fact]
        public void UpdateDiaryNumberTwiceDoNotThrowException()
        {
            string diaryNumber = Guid.NewGuid().ToString();
            var config = Global.GetConfig;
            var api = new AbouRestApi(config);

            api.UpdateDiaryNumber("190227-BSF_10-GC95", diaryNumber);
            api.UpdateDiaryNumber("190227-BSF_10-GC95", diaryNumber);

        }
    }
}