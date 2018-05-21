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
    /// MasspaymentCreateResponse
    /// </summary>
    [DataContract]
    public partial class MasspaymentCreateResponse :  IEquatable<MasspaymentCreateResponse>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MasspaymentCreateResponse" /> class.
        /// </summary>
        /// <param name="Result">Result.</param>
        /// <param name="Sum">Sum of transfers in the package.</param>
        /// <param name="Count">Number of transfers defined in CSV file.</param>
        /// <param name="PackId">PackId.</param>
        /// <param name="Referers">Field visible if transfersID has been sent (see chap. \&quot;Exemplary CSV file\&quot;) in JSON format as following: ID in transfer : ID of transfers in tpay.com system. This allows tracking single transfers. .</param>
        /// <param name="Error">Error.</param>
        /// <param name="Desc">Desc.</param>
        public MasspaymentCreateResponse(Result Result = default(Result), decimal? Sum = default(decimal?), int? Count = default(int?), PackId PackId = default(PackId), string Referers = default(string), MasspaymentErrCode Error = default(MasspaymentErrCode), MasspaymentErrDesc Desc = default(MasspaymentErrDesc))
        {
            this.Result = Result;
            this.Sum = Sum;
            this.Count = Count;
            this.PackId = PackId;
            this.Referers = Referers;
            this.Error = Error;
            this.Desc = Desc;
        }
        
        /// <summary>
        /// Gets or Sets Result
        /// </summary>
        [DataMember(Name="result", EmitDefaultValue=false)]
        public Result Result { get; set; }

        /// <summary>
        /// Sum of transfers in the package
        /// </summary>
        /// <value>Sum of transfers in the package</value>
        [DataMember(Name="sum", EmitDefaultValue=false)]
        public decimal? Sum { get; set; }

        /// <summary>
        /// Number of transfers defined in CSV file
        /// </summary>
        /// <value>Number of transfers defined in CSV file</value>
        [DataMember(Name="count", EmitDefaultValue=false)]
        public int? Count { get; set; }

        /// <summary>
        /// Gets or Sets PackId
        /// </summary>
        [DataMember(Name="pack_id", EmitDefaultValue=false)]
        public PackId PackId { get; set; }

        /// <summary>
        /// Field visible if transfersID has been sent (see chap. \&quot;Exemplary CSV file\&quot;) in JSON format as following: ID in transfer : ID of transfers in tpay.com system. This allows tracking single transfers. 
        /// </summary>
        /// <value>Field visible if transfersID has been sent (see chap. \&quot;Exemplary CSV file\&quot;) in JSON format as following: ID in transfer : ID of transfers in tpay.com system. This allows tracking single transfers. </value>
        [DataMember(Name="referers", EmitDefaultValue=false)]
        public string Referers { get; set; }

        /// <summary>
        /// Gets or Sets Error
        /// </summary>
        [DataMember(Name="error", EmitDefaultValue=false)]
        public MasspaymentErrCode Error { get; set; }

        /// <summary>
        /// Gets or Sets Desc
        /// </summary>
        [DataMember(Name="desc", EmitDefaultValue=false)]
        public MasspaymentErrDesc Desc { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class MasspaymentCreateResponse {\n");
            sb.Append("  Result: ").Append(Result).Append("\n");
            sb.Append("  Sum: ").Append(Sum).Append("\n");
            sb.Append("  Count: ").Append(Count).Append("\n");
            sb.Append("  PackId: ").Append(PackId).Append("\n");
            sb.Append("  Referers: ").Append(Referers).Append("\n");
            sb.Append("  Error: ").Append(Error).Append("\n");
            sb.Append("  Desc: ").Append(Desc).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as MasspaymentCreateResponse);
        }

        /// <summary>
        /// Returns true if MasspaymentCreateResponse instances are equal
        /// </summary>
        /// <param name="input">Instance of MasspaymentCreateResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(MasspaymentCreateResponse input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Result == input.Result ||
                    (this.Result != null &&
                    this.Result.Equals(input.Result))
                ) && 
                (
                    this.Sum == input.Sum ||
                    (this.Sum != null &&
                    this.Sum.Equals(input.Sum))
                ) && 
                (
                    this.Count == input.Count ||
                    (this.Count != null &&
                    this.Count.Equals(input.Count))
                ) && 
                (
                    this.PackId == input.PackId ||
                    (this.PackId != null &&
                    this.PackId.Equals(input.PackId))
                ) && 
                (
                    this.Referers == input.Referers ||
                    (this.Referers != null &&
                    this.Referers.Equals(input.Referers))
                ) && 
                (
                    this.Error == input.Error ||
                    (this.Error != null &&
                    this.Error.Equals(input.Error))
                ) && 
                (
                    this.Desc == input.Desc ||
                    (this.Desc != null &&
                    this.Desc.Equals(input.Desc))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.Result != null)
                    hashCode = hashCode * 59 + this.Result.GetHashCode();
                if (this.Sum != null)
                    hashCode = hashCode * 59 + this.Sum.GetHashCode();
                if (this.Count != null)
                    hashCode = hashCode * 59 + this.Count.GetHashCode();
                if (this.PackId != null)
                    hashCode = hashCode * 59 + this.PackId.GetHashCode();
                if (this.Referers != null)
                    hashCode = hashCode * 59 + this.Referers.GetHashCode();
                if (this.Error != null)
                    hashCode = hashCode * 59 + this.Error.GetHashCode();
                if (this.Desc != null)
                    hashCode = hashCode * 59 + this.Desc.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

}
