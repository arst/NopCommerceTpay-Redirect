/* 
 * Tpay.com Technical Documentation
 *
 *  <p class=\"changes-disclaimer\"> Demo transaction/masspayments api key: <input type=\"text\" id=\"transaction_key\" value=\"75f86137a6635df826e3efe2e66f7c9a946fdde1\" class=\"ui-form-control\"/><label for=\"transaction_key\" style=\"display: none;\" id=\"tr_api_label\">COPIED!</label><br/><br/> Demo cards api key: <input type=\"text\" id=\"cards_key\" value=\"ba9a05faa697f9b43f39b84933ff168e373c6496\" class=\"ui-form-control\"/><label for=\"cards_key\" style=\"display: none;\" id=\"cards_api_label\">COPIED!</label><br/><br/> Demo registration api key: <input type=\"text\" id=\"registration_key\" value=\"6c0f5ef6e4d6877abad7fcfb3b5de117ad8b772d\" class=\"ui-form-control\"/><label for=\"registration_key\" style=\"display: none;\" id=\"registration_api_label\">COPIED!</label><br/><br/> The terms seller and merchant are used interchangeably and they both refer to a person or a company registered at tpay.com to accept online payments. <br/> Whenever term merchant panel is used it refers to the part of tpay.com website located at <a href=\"https://secure.tpay.com/panel\" target=\"_blank\">secure.tpay.com/panel</a>. <br/><br/> For sandbox purposes use merchant demo account <br/><br/> ID - 1010, Password - demo<br/><br/>Remember that this is a shared account, so all data passed through will be publicly visible.</p>
 *
 * OpenAPI spec version: 1.1.1
 * Contact: pt@tpay.com
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using SwaggerDateConverter = IO.Swagger.Client.SwaggerDateConverter;

namespace IO.Swagger.Model
{
    /// <summary>
    /// Error code number if an error occurs or empty.
    /// </summary>
    /// <value>Error code number if an error occurs or empty.</value>
    
    [JsonConverter(typeof(StringEnumConverter))]
    
    public enum CardsErrCode
    {
        
        /// <summary>
        /// Enum _1 for value: 1
        /// </summary>
        [EnumMember(Value = "1")]
        _1 = 1,
        
        /// <summary>
        /// Enum _2 for value: 2
        /// </summary>
        [EnumMember(Value = "2")]
        _2 = 2,
        
        /// <summary>
        /// Enum _3 for value: 3
        /// </summary>
        [EnumMember(Value = "3")]
        _3 = 3,
        
        /// <summary>
        /// Enum _4 for value: 4
        /// </summary>
        [EnumMember(Value = "4")]
        _4 = 4,
        
        /// <summary>
        /// Enum _5 for value: 5
        /// </summary>
        [EnumMember(Value = "5")]
        _5 = 5,
        
        /// <summary>
        /// Enum _6 for value: 6
        /// </summary>
        [EnumMember(Value = "6")]
        _6 = 6,
        
        /// <summary>
        /// Enum _7 for value: 7
        /// </summary>
        [EnumMember(Value = "7")]
        _7 = 7,
        
        /// <summary>
        /// Enum _8 for value: 8
        /// </summary>
        [EnumMember(Value = "8")]
        _8 = 8,
        
        /// <summary>
        /// Enum _9 for value: 9
        /// </summary>
        [EnumMember(Value = "9")]
        _9 = 9,
        
        /// <summary>
        /// Enum _10 for value: 10
        /// </summary>
        [EnumMember(Value = "10")]
        _10 = 10,
        
        /// <summary>
        /// Enum _11 for value: 11
        /// </summary>
        [EnumMember(Value = "11")]
        _11 = 11,
        
        /// <summary>
        /// Enum _12 for value: 12
        /// </summary>
        [EnumMember(Value = "12")]
        _12 = 12,
        
        /// <summary>
        /// Enum _13 for value: 13
        /// </summary>
        [EnumMember(Value = "13")]
        _13 = 13,
        
        /// <summary>
        /// Enum _14 for value: 14
        /// </summary>
        [EnumMember(Value = "14")]
        _14 = 14,
        
        /// <summary>
        /// Enum _15 for value: 15
        /// </summary>
        [EnumMember(Value = "15")]
        _15 = 15,
        
        /// <summary>
        /// Enum _16 for value: 16
        /// </summary>
        [EnumMember(Value = "16")]
        _16 = 16,
        
        /// <summary>
        /// Enum _17 for value: 17
        /// </summary>
        [EnumMember(Value = "17")]
        _17 = 17
    }

}
