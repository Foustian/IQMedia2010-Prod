﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------



[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName="IGeneratePDFWebService")]
public interface IGeneratePDFWebService
{
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneratePDFWebService/WakeupService", ReplyAction="http://tempuri.org/IGeneratePDFWebService/WakeupServiceResponse")]
    void WakeupService();
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public interface IGeneratePDFWebServiceChannel : IGeneratePDFWebService, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public partial class GeneratePDFWebServiceClient : System.ServiceModel.ClientBase<IGeneratePDFWebService>, IGeneratePDFWebService
{
    
    public GeneratePDFWebServiceClient()
    {
    }
    
    public GeneratePDFWebServiceClient(string endpointConfigurationName) : 
            base(endpointConfigurationName)
    {
    }
    
    public GeneratePDFWebServiceClient(string endpointConfigurationName, string remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public GeneratePDFWebServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public GeneratePDFWebServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(binding, remoteAddress)
    {
    }
    
    public void WakeupService()
    {
        base.Channel.WakeupService();
    }
}
