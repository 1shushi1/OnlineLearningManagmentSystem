using System;
using System.Collections.Generic;

public class Certificate
{
    public int CertificateID { get; private set; }
    public DateTime CompletionDate { get; private set; }
    private string _certificateText;

    public string CertificateText => GenerateCertificateText();

    private static List<Certificate> _certificates = new List<Certificate>();

    public Certificate(int certificateID, DateTime completionDate)
    {
        CertificateID = certificateID;
        CompletionDate = completionDate;
        AddToExtent(this);
    }

    private string GenerateCertificateText()
    {
        return $"Certificate of Completion awarded on {CompletionDate.ToShortDateString()}";
    }

    public static void AddToExtent(Certificate certificate)
    {
        _certificates.Add(certificate ?? throw new ArgumentException("Certificate cannot be null."));
    }

    public static IReadOnlyList<Certificate> GetExtent() => _certificates.AsReadOnly();

    public static void ClearExtent() => _certificates.Clear();
}
