namespace ApiSkeleton.Common
{
    [DescriptiveEnumEnforcement(DescriptiveEnumEnforcementAttribute.EnforcementTypeEnum.ThrowException)]
    public enum ResultCode
    {
        Success,
        Fail,
        HttpError,
        BadRequest,
        NotMatchedProtocol,
        InvalidToken,
        UnknownException,
        NotFoundEmail,
        NotMatchedPassword,
    }
}
