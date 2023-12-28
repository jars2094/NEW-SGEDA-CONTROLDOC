﻿using ControlDoc.Components.Components.Modals;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

using Microsoft.JSInterop;
using Telerik.SvgIcons;

namespace ControlDoc.Components.Components.UploadFiles
{
    
    public partial class DragAndDropFilesComponent : ComponentBase
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        [Parameter] public int MaxFileCount { get; set; } = 5; 
        [Parameter] public string[] AllowedExtensions { get; set; } = { ".jpg", ".jpeg", ".png" ,".svg",".pdf",".xlsx",".docx",".xls",".doc"}; 
        [Parameter] public int MaxFileSizeMB { get; set; } = 30; 
        Dictionary<string, string> bootstrapIcons = new Dictionary<string, string>
    {
        { ".aac", "bi bi-filetype-aac" },
        { ".ai", "bi bi-filetype-ai" },
        { ".bmp", "bi bi-filetype-bmp" },
        { ".cs", "bi bi-filetype-cs" },
        { ".css", "bi bi-filetype-css" },
        { ".csv", "bi bi-filetype-csv" },
        { ".doc", "bi bi-filetype-doc" },
        { ".docx", "bi bi-filetype-docx" },
        { ".exe", "bi bi-filetype-exe" },
        { ".gif", "bi bi-filetype-gif" },
        { ".heic", "bi bi-filetype-heic" },
        { ".html", "bi bi-filetype-html" },
        { ".java", "bi bi-filetype-java" },
        { ".jpg", "bi bi-filetype-jpg" },
        { ".js", "bi bi-filetype-js" },
        { ".json", "bi bi-filetype-json" },
        { ".jsx", "bi bi-filetype-jsx" },
        { ".key", "bi bi-filetype-key" },
        { ".m4p", "bi bi-filetype-m4p" },
        { ".md", "bi bi-filetype-md" },
        { ".mdx", "bi bi-filetype-mdx" },
        { ".mov", "bi bi-filetype-mov" },
        { ".mp3", "bi bi-filetype-mp3" },
        { ".mp4", "bi bi-filetype-mp4" },
        { ".otf", "bi bi-filetype-otf" },
        { ".pdf", "bi bi-filetype-pdf" },
        { ".php", "bi bi-filetype-php" },
        { ".png", "bi bi-filetype-png" },
        { ".ppt", "bi bi-filetype-ppt" },
        { ".pptx", "bi bi-filetype-pptx" },
        { ".psd", "bi bi-filetype-psd" },
        { ".py", "bi bi-filetype-py" },
        { ".raw", "bi bi-filetype-raw" },
        { ".rb", "bi bi-filetype-rb" },
        { ".sass", "bi bi-filetype-sass" },
        { ".scss", "bi bi-filetype-scss" },
        { ".sh", "bi bi-filetype-sh" },
        { ".sql", "bi bi-filetype-sql" },
        { ".svg", "bi bi-filetype-svg" },
        { ".tiff", "bi bi-filetype-tiff" },
        { ".tsx", "bi bi-filetype-tsx" },
        { ".ttf", "bi bi-filetype-ttf" },
        { ".txt", "bi bi-filetype-txt" },
        { ".wav", "bi bi-filetype-wav" },
        { ".woff", "bi bi-filetype-woff" },
        { ".xls", "bi bi-filetype-xls" },
        { ".xlsx", "bi bi-filetype-xlsx" },
        { ".xml", "bi bi-filetype-xml" },
        { ".yml", "bi bi-filetype-yml" },
        // Agrega más asociaciones según sea necesario
    };
        
        [Parameter]
        public EventCallback<List<FileInfoData>> OnFileListChanged { get; set; }

        private async Task NotifyFileListChanged()
        {
            await OnFileListChanged.InvokeAsync(fileInfos.ToList());
        }
        private ModalNotificationsComponent notificationModal;


        private string hoverClass = string.Empty;
        private ElementReference dropContainer;
        private InputFile? inputFile;
        private bool isLoading = false;
        private FileInfoData recordToDelete;


        IJSObjectReference? pasteFileModule;
        IJSObjectReference? initializePasteFileFunction;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("initializeFilePaste", dropContainer, inputFile.Element);
            }
        }

        private async Task TriggerFileInputClick()
        {
            await JSRuntime.InvokeVoidAsync("triggerClick", "inputFileElement");
        }

        void OnDragEnter(DragEventArgs e) => hoverClass = "hover";
        void OnDragLeave(DragEventArgs e) => hoverClass = string.Empty;
        private List<FileInfoData> fileInfos = new List<FileInfoData>();

        async Task OnChange(InputFileChangeEventArgs e)
        {
            isLoading = true;
            StateHasChanged();
            foreach (var file in e.GetMultipleFiles())
            {
                if (IsValidFile(file))
                {
                    
                    await ProcessValidFile(file);
                }
                else
                {
                    
                    await HandleInvalidFile(file);
                }
            }

            isLoading = false; 
            StateHasChanged(); 
        }
        private bool IsValidFile(IBrowserFile file)
        {
            return IsSizeValid(file) && IsExtensionValid(file) && IsFileCountValid();
        }

        private async Task ProcessValidFile(IBrowserFile file)
        {
            var fileInfo = await ConvertFileToFileInfo(file);

            if (IsDuplicate(fileInfo))
            {
                
                return;
            }

            fileInfos.Add(fileInfo);
            await NotifyFileListChanged();
        }

        private bool IsDuplicate(FileInfoData newFile)
        {
            // Buscar un archivo con el mismo nombre
            var existingFile = fileInfos.FirstOrDefault(f => f.Name == newFile.Name);

            if (existingFile != null && existingFile.Hash != newFile.Hash)
            {
                
                newFile.Name = GenerateUniqueFileName(newFile.Name);
            }

            
            return existingFile != null;
        }

        private string GenerateUniqueFileName(string originalName)
        {
            var count = 1;
            string newName;

            do
            {
                count++;
                newName = $"{originalName}({count})";
            } while (fileInfos.Any(f => f.Name == newName));

            return newName;
        }

        private async Task HandleInvalidFile(IBrowserFile file)
        {
            notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Error, "Archivo no permitido", "Aceptar", true);
            await NotifyFileListChanged();
        }

        private bool IsSizeValid(IBrowserFile file)
        {
            
            return file.Size <= MaxFileSizeMB * 1024 * 1024;
        }

        private bool IsExtensionValid(IBrowserFile file)
        {
            
            var fileExtension = Path.GetExtension(file.Name).ToLowerInvariant();
            return AllowedExtensions.Contains(fileExtension);
        }

        private bool IsFileCountValid()
        {
            
            return fileInfos.Count < MaxFileCount;
        }

        private async Task<FileInfoData> ConvertFileToFileInfo(IBrowserFile file)
        {
            
            using var stream = file.OpenReadStream(MaxFileSizeMB * 1024 * 1024);
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            var base64Data = Convert.ToBase64String(ms.ToArray());
            //var hash = await CalculateFileHash(file);

            return new FileInfoData
            {
                Name = Path.GetFileNameWithoutExtension(file.Name),
                Extension = Path.GetExtension(file.Name),
                Size = file.Size,
                IconPath = GetBootstrapIconByExtension(Path.GetExtension(file.Name)),
                Base64Data = base64Data,
                //Hash = hash
            };
        }
        private async Task<string> CalculateFileHash(IBrowserFile file)
        {
            using var stream = file.OpenReadStream(MaxFileSizeMB * 1024 * 1024);
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashBytes = await sha256.ComputeHashAsync(stream);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }

        private string GetBootstrapIconByExtension(string extension)
        {
            
            return bootstrapIcons.TryGetValue(extension, out var icon) ? icon : "bi bi-file"; 
        }

        public async ValueTask DisposeAsync()
        {
            if (initializePasteFileFunction is not null)
            {
                await initializePasteFileFunction.InvokeVoidAsync("dispose");
                await initializePasteFileFunction.DisposeAsync();
            }

            if (pasteFileModule is not null)
            {
                await pasteFileModule.DisposeAsync();
            }
        }

        private async Task DeleteFile(FileInfoData fileInfo)
        {
            fileInfos.Remove(fileInfo);
            await NotifyFileListChanged();
        }

        private async Task DeleteFileModalOpen(FileInfoData fileInfo)
        {
            //fileInfos.Remove(fileInfo);
            //await NotifyFileListChanged();
            recordToDelete = fileInfo;
            notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Delete, "¿Está seguro de eliminar este Adjunto?", "Aceptar", true);
        }

        private async Task HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if (args.IsAccepted)
            {
                await DeleteFile(recordToDelete);
            }
            else
            {
                Console.WriteLine("Registro No eliminado");
            }


        }
    }
}
