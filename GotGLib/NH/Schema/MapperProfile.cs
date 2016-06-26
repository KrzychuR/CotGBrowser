using AutoMapper;
using GotGLib.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.NH.Schema
{
    public class MapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<DBRankingsEmpireScore, EmpireScoreHistory>()
                .ForMember(s => s.Errors, opt => opt.Ignore())
                .ForMember(s => s.UISelected, opt => opt.Ignore())
                ;

            Mapper.CreateMap<EmpireScoreHistory, DBRankingsEmpireScore>()
                .ForSourceMember(s => s.Errors, opt => opt.Ignore())
                .ForSourceMember(s => s.UISelected, opt => opt.Ignore())
                .ForSourceMember(s => s.Id, opt => opt.Ignore())
                .ForMember(s => s.Id, opt => opt.Ignore())
                ;

            Mapper.CreateMap<EmpireScoreHistory, EmpireScoreHistory>();

            //---------------------------------

            Mapper.CreateMap<DBCurrentEmpireRanking, CurrentEmpireRanking>()
                .ForMember(s => s.Errors, opt => opt.Ignore())
                .ForMember(s => s.UISelected, opt => opt.Ignore())
                ;

            Mapper.CreateMap<CurrentEmpireRanking, DBCurrentEmpireRanking>()
                .ForSourceMember(s => s.Errors, opt => opt.Ignore())
                .ForSourceMember(s => s.UISelected, opt => opt.Ignore())
                .ForSourceMember(s => s.Id, opt => opt.Ignore())
                .ForMember(s => s.Id, opt => opt.Ignore())
                ;

            Mapper.CreateMap<CurrentEmpireRanking, CurrentEmpireRanking>();

            //---------------------------------

            Mapper.CreateMap<DBUnitsKillsHistory, UnitsKillsHistory>()
                .ForMember(s => s.Errors, opt => opt.Ignore())
                .ForMember(s => s.UISelected, opt => opt.Ignore())
                ;

            Mapper.CreateMap<UnitsKillsHistory, DBUnitsKillsHistory>()
                .ForSourceMember(s => s.Errors, opt => opt.Ignore())
                .ForSourceMember(s => s.UISelected, opt => opt.Ignore())
                .ForSourceMember(s => s.Id, opt => opt.Ignore())
                .ForMember(s => s.Id, opt => opt.Ignore())
                ;

            Mapper.CreateMap<UnitsKillsHistory, UnitsKillsHistory>();

            //---------------------------------

            Mapper.CreateMap<DBAlianceScoreHistory, AlianceScoreHistory>()
                .ForMember(s => s.Errors, opt => opt.Ignore())
                .ForMember(s => s.UISelected, opt => opt.Ignore())
                ;

            Mapper.CreateMap<AlianceScoreHistory, DBAlianceScoreHistory>()
                .ForSourceMember(s => s.Errors, opt => opt.Ignore())
                .ForSourceMember(s => s.UISelected, opt => opt.Ignore())
                .ForSourceMember(s => s.Id, opt => opt.Ignore())
                .ForMember(s => s.Id, opt => opt.Ignore())
                ;

            Mapper.CreateMap<AlianceScoreHistory, AlianceScoreHistory>();

            //---------------------------------

            Mapper.CreateMap<DBCurrentAlianceRanking, CurrentAlianceRanking>()
                .ForMember(s => s.Errors, opt => opt.Ignore())
                .ForMember(s => s.UISelected, opt => opt.Ignore())
                ;

            Mapper.CreateMap<CurrentAlianceRanking, DBCurrentAlianceRanking>()
                .ForSourceMember(s => s.Errors, opt => opt.Ignore())
                .ForSourceMember(s => s.UISelected, opt => opt.Ignore())
                .ForSourceMember(s => s.Id, opt => opt.Ignore())
                .ForMember(s => s.Id, opt => opt.Ignore())
                ;

            Mapper.CreateMap<CurrentAlianceRanking, CurrentAlianceRanking>();

            //---------------------------------

            Mapper.CreateMap<DBCavernsHistory, CavernsHistory>()
                .ForMember(s => s.Errors, opt => opt.Ignore())
                .ForMember(s => s.UISelected, opt => opt.Ignore())
                ;

            Mapper.CreateMap<CavernsHistory, DBCavernsHistory>()
                .ForSourceMember(s => s.Errors, opt => opt.Ignore())
                .ForSourceMember(s => s.UISelected, opt => opt.Ignore())
                .ForSourceMember(s => s.Id, opt => opt.Ignore())
                .ForMember(s => s.Id, opt => opt.Ignore())
                ;

            Mapper.CreateMap<CavernsHistory, CavernsHistory>();

            //---------------------------------

            Mapper.CreateMap<DBOffReputationHistory, OffReputationHistory>()
                .ForMember(s => s.Errors, opt => opt.Ignore())
                .ForMember(s => s.UISelected, opt => opt.Ignore())
                ;

            Mapper.CreateMap<OffReputationHistory, DBOffReputationHistory>()
                .ForSourceMember(s => s.Errors, opt => opt.Ignore())
                .ForSourceMember(s => s.UISelected, opt => opt.Ignore())
                .ForSourceMember(s => s.Id, opt => opt.Ignore())
                .ForMember(s => s.Id, opt => opt.Ignore())
                ;

            Mapper.CreateMap<OffReputationHistory, OffReputationHistory>();

            //---------------------------------

            Mapper.CreateMap<DBDefReputationHistory, DefReputationHistory>()
                .ForMember(s => s.Errors, opt => opt.Ignore())
                .ForMember(s => s.UISelected, opt => opt.Ignore())
                ;

            Mapper.CreateMap<DefReputationHistory, DBDefReputationHistory>()
                .ForSourceMember(s => s.Errors, opt => opt.Ignore())
                .ForSourceMember(s => s.UISelected, opt => opt.Ignore())
                .ForSourceMember(s => s.Id, opt => opt.Ignore())
                .ForMember(s => s.Id, opt => opt.Ignore())
                ;

            Mapper.CreateMap<DefReputationHistory, DefReputationHistory>();

            //---------------------------------

            Mapper.CreateMap<DBPlayer, Player>()
                .ForMember(s => s.Errors, opt => opt.Ignore())
                .ForMember(s => s.UISelected, opt => opt.Ignore())
                ;

            Mapper.CreateMap<Player, DBPlayer>()
                .ForSourceMember(s => s.Errors, opt => opt.Ignore())
                .ForSourceMember(s => s.UISelected, opt => opt.Ignore())
                .ForSourceMember(s => s.Id, opt => opt.Ignore())
                .ForMember(s => s.Id, opt => opt.Ignore())
                ;

            Mapper.CreateMap<Player, Player>();

            //---------------------------------
            Mapper.CreateMap<Resources, Resources>();
            Mapper.CreateMap<ReportTrops, ReportTrops>();
            Mapper.CreateMap<RaidReport, RaidReport>();
        }
    }
}