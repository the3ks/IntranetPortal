namespace IntranetPortal.Data.Models.Assets
{
    public enum RequestStatus
    {
        PendingApproval,
        PendingFulfillment,
        InProcurement,
        Fulfilled,
        Rejected,
        Cancelled
    }

    public enum RequestType
    {
        SerializedAsset,
        BulkAccessory
    }
}
