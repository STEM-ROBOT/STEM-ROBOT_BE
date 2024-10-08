using AutoMapper;
using Microsoft.AspNetCore.SignalR;

using STEM_ROBOT.Common.Req;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Models;
using STEM_ROBOT.DAL.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.BLL.Svc
{
    public class TournamentSvc 
    {
        public IMapper _mapper;
        public TournamentRepo _tournament;
      //  public IHubContext<TournamentClient> _hubContext;
        public TournamentSvc(TournamentRepo tournamentRepo, IMapper mapper) 
        {
        _mapper = mapper;
        _tournament = tournamentRepo;
           
        }

        public SingleRsp AddTournement(int userID, TournamentReq request)
        {
            var res = new SingleRsp();
            try
            {
                if(request.StartDate <  DateTime.Now || request.EndDate < request.StartDate) {

                    res.SetError("400", "Invalid registration dates");
                    return res;
                }
                var tournament = _mapper.Map<Tournament>(request);
                tournament.AccountId = userID;

                _tournament.Add(tournament);
                res.setData("200", tournament);


            }
            catch(Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        //public async Task<SingleRsp> CountTimeTournament(int tourNamentID)
        //{
        //    var res = new SingleRsp();
        //    try
        //    {
        //        var tournament = _tournament.getID(tourNamentID);
        //        if(tournament == null)
        //        {
        //            res.SetError("No TournamentID");

        //        }
        //        await _hubContext.Clients.All.SendAsync("ReceiveCountdown", tournament.EndDate);

        //    }
        //    catch(Exception ex)
        //    {
        //        res.SetError("500", ex.Message);
        //    }
        //    return res;
        //}
    }
}
