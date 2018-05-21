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
    /// DeregisterFields
    /// </summary>
    [DataContract]
    public partial class DeregisterFields :  IEquatable<DeregisterFields>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeregisterFields" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected DeregisterFields() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="DeregisterFields" /> class.
        /// </summary>
        /// <param name="CliAuth">CliAuth (required).</param>
        /// <param name="Language">Language.</param>
        /// <param name="ApiPassword">ApiPassword (required).</param>
        /// <param name="Sign">Sign is calculated from cryptographic hash function set in Merchant’s Panel (default SHA-1): hash_alg (method + cli_auth + language + verification code) where + means concatenation. (required).</param>
        public DeregisterFields(CliAuth CliAuth = default(CliAuth), Language Language = default(Language), CardApiPassword ApiPassword = default(CardApiPassword), string Sign = default(string))
        {
            // to ensure "CliAuth" is required (not null)
            if (CliAuth == null)
            {
                throw new InvalidDataException("CliAuth is a required property for DeregisterFields and cannot be null");
            }
            else
            {
                this.CliAuth = CliAuth;
            }
            // to ensure "ApiPassword" is required (not null)
            if (ApiPassword == null)
            {
                throw new InvalidDataException("ApiPassword is a required property for DeregisterFields and cannot be null");
            }
            else
            {
                this.ApiPassword = ApiPassword;
            }
            // to ensure "Sign" is required (not null)
            if (Sign == null)
            {
                throw new InvalidDataException("Sign is a required property for DeregisterFields and cannot be null");
            }
            else
            {
                this.Sign = Sign;
            }
            this.Language = Language;
        }
        
        /// <summary>
        /// Gets or Sets CliAuth
        /// </summary>
        [DataMember(Name="cli_auth", EmitDefaultValue=false)]
        public CliAuth CliAuth { get; set; }

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
        /// Sign is calculated from cryptographic hash function set in Merchant’s Panel (default SHA-1): hash_alg (method + cli_auth + language + verification code) where + means concatenation.
        /// </summary>
        /// <value>Sign is calculated from cryptographic hash function set in Merchant’s Panel (default SHA-1): hash_alg (method + cli_auth + language + verification code) where + means concatenation.</value>
        [DataMember(Name="sign", EmitDefaultValue=false)]
        public string Sign { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class DeregisterFields {\n");
            sb.Append("  CliAuth: ").Append(CliAuth).Append("\n");
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
            return this.Equals(input as DeregisterFields);
        }

        /// <summary>
        /// Returns true if DeregisterFields instances are equal
        /// </summary>
        /// <param name="input">Instance of DeregisterFields to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(DeregisterFields input)
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
            yield break;
        }
    }

}
