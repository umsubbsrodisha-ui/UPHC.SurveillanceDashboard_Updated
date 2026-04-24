
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using UPHC.SurveillanceDashboard.Data;
using UPHC.SurveillanceDashboard.Models;
using UPHC.SurveillanceDashboard.Hubs;

namespace UPHC.SurveillanceDashboard.Services
{
    public class NotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IDbContextFactory<AppDbContext> _dbFactory;

        public NotificationService(
            IHubContext<NotificationHub> hubContext,
            IDbContextFactory<AppDbContext> dbFactory)
        {
            _hubContext = hubContext;
            _dbFactory = dbFactory;
        }

        // ✅ sending notification
        public async Task SendNotification(int caseRecordId, string type)
        {
            using var db = _dbFactory.CreateDbContext();

            var caseRecord = await db.CaseRecords
                .Include(c => c.UPHC)
                .FirstOrDefaultAsync(c => c.Id == caseRecordId);

            if (caseRecord == null) return;

            //  Save notification
            var notification = new Notification
            {
                CaseRecordId = caseRecord.Id,
                UPHCId = caseRecord.UPHCId,
                DiseaseName = caseRecord.DiseaseName,
                Type = type,
                IsChecked = false
            };

            db.Notifications.Add(notification);
            await db.SaveChangesAsync();

            // SEND FULL DATA 
            await _hubContext.Clients.Group("Analysts")
                .SendAsync(
                    "ReceiveNotification",
                    notification.Id,                 // for marking read
                    notification.Type,               // Suspected / Confirmed / Negative
                    caseRecord.DiseaseName,          // Disease
                    caseRecord.UPHC.Name             // UPHC
                );
        }

        //Checked
        public async Task MarkAsChecked(int id)
        {
            using var db = _dbFactory.CreateDbContext();

            var n = await db.Notifications.FindAsync(id);
            if (n != null)
            {
                n.IsChecked = true;
                await db.SaveChangesAsync();
            }
        }




        //  Dashboard refresh trigger
        public async Task NotifyDashboardUpdate()
        {
            await _hubContext.Clients.Group("Dashboard")
                .SendAsync("DashboardUpdated");
        }







    }
}








































//    using Microsoft.AspNetCore.SignalR;
//    using Microsoft.EntityFrameworkCore;
//    using UPHC.SurveillanceDashboard.Data;
//    using UPHC.SurveillanceDashboard.Models;

//    using UPHC.SurveillanceDashboard.Hubs;


//namespace UPHC.SurveillanceDashboard.Services
//{




//    public class NotificationService
//    {
//        private readonly IHubContext<NotificationHub> _hubContext;
//        private readonly IDbContextFactory<AppDbContext> _dbFactory;

//        public NotificationService(IHubContext<NotificationHub> hubContext, IDbContextFactory<AppDbContext> dbFactory)
//        {
//            _hubContext = hubContext;
//            _dbFactory = dbFactory;
//        }



//        public async Task SendNotification(int caseRecordId, string type)        //<-----------static polymorphism
//        {
//            using var db = _dbFactory.CreateDbContext();

//            var caseRecord = await db.CaseRecords
//                .Include(c => c.UPHC)
//                .FirstOrDefaultAsync(c => c.Id == caseRecordId);

//            if (caseRecord == null) return;

//            var notification = new Notification
//            {
//                CaseRecordId = caseRecord.Id,
//                UPHCId = caseRecord.UPHCId,
//                DiseaseName = caseRecord.DiseaseName,
//                Type = type
//            };

//            db.Notifications.Add(notification);
//            await db.SaveChangesAsync();

//            //await _hubContext.Clients.Group("Analysts")
//            //    .SendAsync("ReceiveNotification",
//            //        caseRecord.UPHC.Name,
//            //        caseRecord.DiseaseName);
//            await _hubContext.Clients.Group("Analysts")
//    .SendAsync("ReceiveNotification", notification.Id);
//        }

//        public async Task MarkAsChecked(int id)
//        {
//            using var db = _dbFactory.CreateDbContext();

//            var n = await db.Notifications.FindAsync(id);
//            if (n != null)
//            {
//                n.IsChecked = true;
//                await db.SaveChangesAsync();
//            }
//        }
//    }


//}


