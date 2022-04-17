namespace CourseDemo.DTO.Response
{
    public class ImportExcelResponseDto
    {
        public int RecordInserted { get; set; } = 0;
        public int RecordFail { get; set; } = 0;
        public ImportExcelResponseDto()
        {
        }
        public ImportExcelResponseDto(int recordInserted, int recordFail)
        {
            RecordInserted = recordInserted;
            RecordFail = recordFail;
        }
    }
}
