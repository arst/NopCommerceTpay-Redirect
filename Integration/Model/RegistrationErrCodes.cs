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
    /// error code (see registration errors description table)
    /// </summary>
    /// <value>error code (see registration errors description table)</value>
    
    [JsonConverter(typeof(StringEnumConverter))]
    
    public enum RegistrationErrCodes
    {
        
        /// <summary>
        /// Enum ERR71 for value: ERR71
        /// </summary>
        [EnumMember(Value = "ERR71")]
        ERR71 = 1,
        
        /// <summary>
        /// Enum ERR72 for value: ERR72
        /// </summary>
        [EnumMember(Value = "ERR72")]
        ERR72 = 2,
        
        /// <summary>
        /// Enum ERR73 for value: ERR73
        /// </summary>
        [EnumMember(Value = "ERR73")]
        ERR73 = 3,
        
        /// <summary>
        /// Enum ERR74 for value: ERR74
        /// </summary>
        [EnumMember(Value = "ERR74")]
        ERR74 = 4,
        
        /// <summary>
        /// Enum ERR97 for value: ERR97
        /// </summary>
        [EnumMember(Value = "ERR97")]
        ERR97 = 5
    }

}