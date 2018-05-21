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
    /// SaleResponse
    /// </summary>
    [DataContract]
    public partial class SaleResponse :  IEquatable<SaleResponse>, IValidatableObject
    {
        /// <summary>
        /// Defines Status
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum StatusEnum
        {
            
            /// <summary>
            /// Enum Correct for value: correct
            /// </summary>
            [EnumMember(Value = "correct")]
            Correct = 1,
            
            /// <summary>
            /// Enum Declined for value: declined
            /// </summary>
            [EnumMember(Value = "declined")]
            Declined = 2,
            
            /// <summary>
            /// Enum Done for value: done
            /// </summary>
            [EnumMember(Value = "done")]
            Done = 3
        }

        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [DataMember(Name="status", EmitDefaultValue=false)]
        public StatusEnum Status { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="SaleResponse" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected SaleResponse() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="SaleResponse" /> class.
        /// </summary>
        /// <param name="Result">Result (required).</param>
        /// <param name="TestMode">TestMode.</param>
        /// <param name="SaleAuth">SaleAuth (required).</param>
        /// <param name="CliAuth">CliAuth.</param>
        /// <param name="Currency">Currency (required).</param>
        /// <param name="Amount">Amount (required).</param>
        /// <param name="Date">Date.</param>
        /// <param name="Status">Status (required).</param>
        /// <param name="Reason">Reason.</param>
        /// <param name="Sign">Response sign &#x3D; hash_alg(sale_auth + cli_auth + currency + amount + sale_date+ status + reason + verification code) (required).</param>
        public SaleResponse(Result Result = default(Result), int? TestMode = default(int?), SaleAuth SaleAuth = default(SaleAuth), CliAuth CliAuth = default(CliAuth), Currency Currency = default(Currency), Amount Amount = default(Amount), DateTime? Date = default(DateTime?), StatusEnum Status = default(StatusEnum), CardsRejectionReason Reason = default(CardsRejectionReason), string Sign = default(string))
        {
            // to ensure "Result" is required (not null)
            if (Result == null)
            {
                throw new InvalidDataException("Result is a required property for SaleResponse and cannot be null");
            }
            else
            {
                this.Result = Result;
            }
            // to ensure "SaleAuth" is required (not null)
            if (SaleAuth == null)
            {
                throw new InvalidDataException("SaleAuth is a required property for SaleResponse and cannot be null");
            }
            else
            {
                this.SaleAuth = SaleAuth;
            }
            // to ensure "Currency" is required (not null)
            if (Currency == null)
            {
                throw new InvalidDataException("Currency is a required property for SaleResponse and cannot be null");
            }
            else
            {
                this.Currency = Currency;
            }
            // to ensure "Amount" is required (not null)
            if (Amount == null)
            {
                throw new InvalidDataException("Amount is a required property for SaleResponse and cannot be null");
            }
            else
            {
                this.Amount = Amount;
            }
            // to ensure "Status" is required (not null)
            if (Status == null)
            {
                throw new InvalidDataException("Status is a required property for SaleResponse and cannot be null");
            }
            else
            {
                this.Status = Status;
            }
            // to ensure "Sign" is required (not null)
            if (Sign == null)
            {
                throw new InvalidDataException("Sign is a required property for SaleResponse and cannot be null");
            }
            else
            {
                this.Sign = Sign;
            }
            this.TestMode = TestMode;
            this.CliAuth = CliAuth;
            this.Date = Date;
            this.Reason = Reason;
        }
        
        /// <summary>
        /// Gets or Sets Result
        /// </summary>
        [DataMember(Name="result", EmitDefaultValue=false)]
        public Result Result { get; set; }

        /// <summary>
        /// Gets or Sets TestMode
        /// </summary>
        [DataMember(Name="test_mode", EmitDefaultValue=false)]
        public int? TestMode { get; set; }

        /// <summary>
        /// Gets or Sets SaleAuth
        /// </summary>
        [DataMember(Name="sale_auth", EmitDefaultValue=false)]
        public SaleAuth SaleAuth { get; set; }

        /// <summary>
        /// Gets or Sets CliAuth
        /// </summary>
        [DataMember(Name="cli_auth", EmitDefaultValue=false)]
        public CliAuth CliAuth { get; set; }

        /// <summary>
        /// Gets or Sets Currency
        /// </summary>
        [DataMember(Name="currency", EmitDefaultValue=false)]
        public Currency Currency { get; set; }

        /// <summary>
        /// Gets or Sets Amount
        /// </summary>
        [DataMember(Name="amount", EmitDefaultValue=false)]
        public Amount Amount { get; set; }

        /// <summary>
        /// Gets or Sets Date
        /// </summary>
        [DataMember(Name="date", EmitDefaultValue=false)]
        public DateTime? Date { get; set; }


        /// <summary>
        /// Gets or Sets Reason
        /// </summary>
        [DataMember(Name="reason", EmitDefaultValue=false)]
        public CardsRejectionReason Reason { get; set; }

        /// <summary>
        /// Response sign &#x3D; hash_alg(sale_auth + cli_auth + currency + amount + sale_date+ status + reason + verification code)
        /// </summary>
        /// <value>Response sign &#x3D; hash_alg(sale_auth + cli_auth + currency + amount + sale_date+ status + reason + verification code)</value>
        [DataMember(Name="sign", EmitDefaultValue=false)]
        public string Sign { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class SaleResponse {\n");
            sb.Append("  Result: ").Append(Result).Append("\n");
            sb.Append("  TestMode: ").Append(TestMode).Append("\n");
            sb.Append("  SaleAuth: ").Append(SaleAuth).Append("\n");
            sb.Append("  CliAuth: ").Append(CliAuth).Append("\n");
            sb.Append("  Currency: ").Append(Currency).Append("\n");
            sb.Append("  Amount: ").Append(Amount).Append("\n");
            sb.Append("  Date: ").Append(Date).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  Reason: ").Append(Reason).Append("\n");
            sb.Append("  Sign: ").Append(Sign).Append("\n");
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
            return this.Equals(input as SaleResponse);
        }

        /// <summary>
        /// Returns true if SaleResponse instances are equal
        /// </summary>
        /// <param name="input">Instance of SaleResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(SaleResponse input)
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
                    this.TestMode == input.TestMode ||
                    (this.TestMode != null &&
                    this.TestMode.Equals(input.TestMode))
                ) && 
                (
                    this.SaleAuth == input.SaleAuth ||
                    (this.SaleAuth != null &&
                    this.SaleAuth.Equals(input.SaleAuth))
                ) && 
                (
                    this.CliAuth == input.CliAuth ||
                    (this.CliAuth != null &&
                    this.CliAuth.Equals(input.CliAuth))
                ) && 
                (
                    this.Currency == input.Currency ||
                    (this.Currency != null &&
                    this.Currency.Equals(input.Currency))
                ) && 
                (
                    this.Amount == input.Amount ||
                    (this.Amount != null &&
                    this.Amount.Equals(input.Amount))
                ) && 
                (
                    this.Date == input.Date ||
                    (this.Date != null &&
                    this.Date.Equals(input.Date))
                ) && 
                (
                    this.Status == input.Status ||
                    (this.Status != null &&
                    this.Status.Equals(input.Status))
                ) && 
                (
                    this.Reason == input.Reason ||
                    (this.Reason != null &&
                    this.Reason.Equals(input.Reason))
                ) && 
                (
                    this.Sign == input.Sign ||
                    (this.Sign != null &&
                    this.Sign.Equals(input.Sign))
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
                if (this.TestMode != null)
                    hashCode = hashCode * 59 + this.TestMode.GetHashCode();
                if (this.SaleAuth != null)
                    hashCode = hashCode * 59 + this.SaleAuth.GetHashCode();
                if (this.CliAuth != null)
                    hashCode = hashCode * 59 + this.CliAuth.GetHashCode();
                if (this.Currency != null)
                    hashCode = hashCode * 59 + this.Currency.GetHashCode();
                if (this.Amount != null)
                    hashCode = hashCode * 59 + this.Amount.GetHashCode();
                if (this.Date != null)
                    hashCode = hashCode * 59 + this.Date.GetHashCode();
                if (this.Status != null)
                    hashCode = hashCode * 59 + this.Status.GetHashCode();
                if (this.Reason != null)
                    hashCode = hashCode * 59 + this.Reason.GetHashCode();
                if (this.Sign != null)
                    hashCode = hashCode * 59 + this.Sign.GetHashCode();
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
            // Sign (string) maxLength
            if(this.Sign != null && this.Sign.Length > 128)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Sign, length must be less than 128.", new [] { "Sign" });
            }

            // Sign (string) minLength
            if(this.Sign != null && this.Sign.Length < 40)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Sign, length must be greater than 40.", new [] { "Sign" });
            }

            yield break;
        }
    }

}
