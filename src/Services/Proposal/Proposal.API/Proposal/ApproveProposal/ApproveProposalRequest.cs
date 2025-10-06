namespace ProposalApi.Proposal.ApproveProposal;

public class ApproveProposalRequest
{
    public decimal ApprovedAmount { get; init; }
    public string? ApprovedCurrency { get; init; } = "BRL";
}