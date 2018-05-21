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
    /// RefundFields
    /// </summary>
    [DataContract]
    public partial class RefundFields :  IEquatable<RefundFields>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefundFields" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected RefundFields() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="RefundFields" /> class.
        /// </summary>
        /// <param name="CliAuth">CliAuth.</param>
        /// <param name="SaleAuth">SaleAuth.</param>
        /// <param name="Desc">Desc (required).</param>
        /// <param name="Currency">Currency.</param>
        /// <param name="Amount">Amount.</param>
        /// <param name="Language">Language.</param>
        /// <param name="ApiPassword">ApiPassword (required).</param>
        /// <param name="Sign">Sign is calculated from cryptographic hash function set in Merchant’s Panel (default SHA-1): hash_alg (method + cli_auth + sale_auth + desc + amount + currency + language + verification code); where + means concatenation.  (required).</param>
        public RefundFields(CliAuth CliAuth = default(CliAuth), SaleAuth SaleAuth = default(SaleAuth), string Desc = default(string), Currency Currency = default(Currency), Amount Amount = default(Amount), Language Language = default(Language), CardApiPassword ApiPassword = default(CardApiPassword), string Sign = default(string))
        {
            // to ensure "Desc" is required (not null)
            if (Desc == null)
            {
                throw new InvalidDataException("Desc is a required property for RefundFields and cannot be null");
            }
            else
            {
                this.Desc = Desc;
            }
            // to ensure "ApiPassword" is required (not null)
            if (ApiPassword == null)
            {
                throw new InvalidDataException("ApiPassword is a required property for RefundFields and cannot be null");
            }
            else
            {
                this.ApiPassword = ApiPassword;
            }
            // to ensure "Sign" is required (not null)
            if (Sign == null)
            {
                throw new InvalidDataException("Sign is a required property for RefundFields and cannot be null");
            }
            else
            {
                this.Sign = Sign;
            }
            this.CliAuth = CliAuth;
            this.SaleAuth = SaleAuth;
            this.Currency = Currency;
            this.Amount = Amount;
            this.Language = Language;
        }
        
        /// <summary>
        /// Gets or Sets CliAuth
        /// </summary>
        [DataMember(Name="cli_auth", EmitDefaultValue=false)]
        public CliAuth CliAuth { get; set; }

        /// <summary>
        /// Gets or Sets SaleAuth
        /// </summary>
        [DataMember(Name="sale_auth", EmitDefaultValue=false)]
        public SaleAuth SaleAuth { get; set; }

        /// <summary>
        /// Gets or Sets Desc
        /// </summary>
        [DataMember(Name="desc", EmitDefaultValue=false)]
        public string Desc { get; set; }

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
        /// Gets or Sets Language
        /// </summary>
        [DataMember(Name="language", EmitDefaultValue=false)]
        public Language Language { get; set; }

        /// <summary>
        /// Gets or Sets ApiPassword
        /// </summary>
        [DataMember(Name="api_password", EmitDefaultValue=false)]
        public CardApiPassword ApiPassword { get; set; }

        /// <summary>
        /// Sign is calculated from cryptographic hash function set in Merchant’s Panel (default SHA-1): hash_alg (method + cli_auth + sale_auth + desc + amount + currency + language + verification code); where + means concatenation. 
        /// </summary>
        /// <value>Sign is calculated from cryptographic hash function set in Merchant’s Panel (default SHA-1): hash_alg (method + cli_auth + sale_auth + desc + amount + currency + language + verification code); where + means concatenation. </value>
        [DataMember(Name="sign", EmitDefaultValue=false)]
        public string Sign { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class RefundFields {\n");
            sb.Append("  CliAuth: ").Append(CliAuth).Append("\n");
            sb.Append("  SaleAuth: ").Append(SaleAuth).Append("\n");
            sb.Append("  Desc: ").Append(Desc).Append("\n");
            sb.Append("  Currency: ").Append(Currency).Append("\n");
            sb.Append("  Amount: ").Append(Amount).Append("\n");
            sb.Append("  Language: ").Append(Language).Append("\n");
            sb.Append("  ApiPassword: ").Append(ApiPassword).Append("\n");
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
            return this.Equals(input as RefundFields);
        }

        /// <summary>
        /// Returns true if RefundFields instances are equal
        /// </summary>
        /// <param name="input">Instance of RefundFields to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RefundFields input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.CliAuth == input.CliAuth ||
                    (this.CliAuth != null &&
                    this.CliAuth.Equals(input.CliAuth))
                ) && 
                (
                    this.SaleAuth == input.SaleAuth ||
                    (this.SaleAuth != null &&
                    this.SaleAuth.Equals(input.SaleAuth))
                ) && 
                (
                    this.Desc == input.Desc ||
                    (this.Desc != null &&
                    this.Desc.Equals(input.Desc))
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
                    this.Language == input.Language ||
                    (this.Language != null &&
                    this.Language.Equals(input.Language))
                ) && 
                (
                    this.ApiPassword == input.ApiPassword ||
                    (this.ApiPassword != null &&
                    this.ApiPassword.Equals(input.ApiPassword))
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
                if (this.CliAuth != null)
                    hashCode = hashCode * 59 + this.CliAuth.GetHashCode();
                if (this.SaleAuth != null)
                    hashCode = hashCode * 59 + this.SaleAuth.GetHashCode();
                if (this.Desc != null)
                    hashCode = hashCode * 59 + this.Desc.GetHashCode();
                if (this.Currency != null)
                    hashCode = hashCode * 59 + this.Currency.GetHashCode();
                if (this.Amount != null)
                    hashCode = hashCode * 59 + this.Amount.GetHashCode();
                if (this.Language != null)
                    hashCode = hashCode * 59 + this.Language.GetHashCode();
                if (this.ApiPassword != null)
                    hashCode = hashCode * 59 + this.ApiPassword.GetHashCode();
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
            // Desc (string) maxLength
            if(this.Desc != null && this.Desc.Length > 128)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Desc, length must be less than 128.", new [] { "Desc" });
            }

            // Desc (string) minLength
            if(this.Desc != null && this.Desc.Length < 1)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Desc, length must be greater than 1.", new [] { "Desc" });
            }

            yield break;
        }
    }

}
