﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.296
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UnitTest.BNLPService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BNLPService.BNLPServiceSoap")]
    public interface BNLPServiceSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Translate", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        string Translate(string Text);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/TranslateAnalysis", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        string TranslateAnalysis(string Text);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/TranslateSite", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        string TranslateSite(string Text);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface BNLPServiceSoapChannel : UnitTest.BNLPService.BNLPServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BNLPServiceSoapClient : System.ServiceModel.ClientBase<UnitTest.BNLPService.BNLPServiceSoap>, UnitTest.BNLPService.BNLPServiceSoap {
        
        public BNLPServiceSoapClient() {
        }
        
        public BNLPServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public BNLPServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BNLPServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BNLPServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string Translate(string Text) {
            return base.Channel.Translate(Text);
        }
        
        public string TranslateAnalysis(string Text) {
            return base.Channel.TranslateAnalysis(Text);
        }
        
        public string TranslateSite(string Text) {
            return base.Channel.TranslateSite(Text);
        }
    }
}