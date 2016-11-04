namespace HKSJ.WBVV.Entity.Document
{
    /// <summary>
    /// Api文档参数
    /// Author:AxOne
    /// </summary>
    public class ApiDocumentParameter
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public bool IsOptional { get; set; }

        public string Description { get; set; }
    }
}
