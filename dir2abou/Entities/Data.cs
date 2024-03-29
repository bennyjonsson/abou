﻿//// <auto-generated />
////
//// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
////
////    using dir2abou.Entities;
////
////    var data = Data.FromJson(jsonString);

//namespace AbouApi.Entities
//{
//    using System;
//    using System.Collections.Generic;

//    using System.Globalization;
//    using Newtonsoft.Json;
//    using Newtonsoft.Json.Converters;

//    public partial class Data
//    {
//        public Data(IActor actor)
//        {
//            Actor = actor.Actor;
//        }
//        [JsonProperty("UniqueId")]
//        public string UniqueId { get; set; }

//        [JsonProperty("DiaryNumber")]
//        public string DiaryNumber { get; set; }

//        [JsonProperty("Comment")]
//        public string Comment { get; set; }

//        [JsonProperty("Actor")]
//        public string Actor { get; set; }
//    }
//    public partial class DataUpdateStatus : Data
//    {
//        public DataUpdateStatus(IActor actor) : base(actor) { }

//        [JsonProperty("DisableSendNotification")]
//        public bool DisableSendNotification { get; set; }

        
//    }

//        public partial class DataGetDetailed : Data
//    {
//        public DataGetDetailed(IActor actor) : base(actor) {
//            Comment = "Get details";
//            XmlGeneratorType = "Abou.Calamare.Framework.Integration.Xml.Default.DefaultXmlCaseGenerator";
//            OmitXmlDeclaration = "true";
//            IncludeAllAttachements = "true";
//            IncludeAllFields = "true";
//        }


//        [JsonProperty("XmlGeneratorType")]
//        public string XmlGeneratorType { get; set; }

//        [JsonProperty("IncludeAllFields")]
//        public string IncludeAllFields { get; set; }

//        [JsonProperty("IncludeAllAttachements")]
//        public string IncludeAllAttachements { get; set; }

//        [JsonProperty("OmitXmlDeclaration")]
//        public string OmitXmlDeclaration { get; set; }
//    }

//    public partial class Data
//    {
//        public static Data FromJson(string json) => JsonConvert.DeserializeObject<Data>(json, dir2abou.Entities.Converter.Settings);
//    }

//    public static class SerializeData
//    {
//        public static string ToJson(this Data self) => JsonConvert.SerializeObject(self, dir2abou.Entities.Converter.Settings);
//    }

    
//}
