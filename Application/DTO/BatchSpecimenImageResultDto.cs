namespace Application.DTO
{
    /// <summary>
    /// DTO с результатами пакетной загрузки изображений
    /// </summary>
    public class BatchSpecimenImageResultDto
    {
        /// <summary>
        /// Идентификатор образца
        /// </summary>
        public int SpecimenId { get; set; }
        
        /// <summary>
        /// Количество успешно загруженных изображений
        /// </summary>
        public int SuccessCount { get; set; }
        
        /// <summary>
        /// Количество ошибок при загрузке
        /// </summary>
        public int ErrorCount { get; set; }
        
        /// <summary>
        /// Коллекция идентификаторов загруженных изображений
        /// </summary>
        public List<int> UploadedImageIds { get; set; } = new List<int>();
        
        /// <summary>
        /// Сообщения об ошибках при загрузке
        /// </summary>
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
} 