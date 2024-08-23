using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Net;
namespace api.Filter
{
    public class BlockIpAttribute : ActionFilterAttribute
    {
        private readonly string[] _blockedIps;
        
        // Nhận danh sách các IP bị chặn qua constructor
        public BlockIpAttribute(params string[] blockedIps)
        {
            _blockedIps = blockedIps;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Lấy IP của người gửi request
            var requestIpAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            // Kiểm tra nếu IP nằm trong danh sách bị chặn
            if (_blockedIps.Contains(requestIpAddress))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 403, // Forbidden
                    Content = "Access denied from this IP address."
                };
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}

