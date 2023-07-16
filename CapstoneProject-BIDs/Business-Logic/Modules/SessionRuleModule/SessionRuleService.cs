using Business_Logic.Modules.SessionRuleModule.Interface;
using Business_Logic.Modules.SessionRuleModule.Request;
using Business_Logic.Modules.UserModule.Interface;
using Data_Access.Constant;
using Data_Access.Entities;
using FluentValidation.Results;

namespace Business_Logic.Modules.SessionRuleModule
{
    public class SessionRuleService : ISessionRuleService
    {
        private readonly ISessionRuleRepository _SessionRuleRepository;
        public SessionRuleService(ISessionRuleRepository SessionRuleRepository)
        {
            _SessionRuleRepository = SessionRuleRepository;
        }

        public async Task<ICollection<SessionRule>> GetAll()
        {
            return await _SessionRuleRepository.GetAll();
        }

        public Task<ICollection<SessionRule>> GetSessionRulesIsValid()
        {
            return _SessionRuleRepository.GetSessionRulesBy(x => x.Status == true);
        }

        public async Task<SessionRule> GetSessionRuleByID(Guid id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var SessionRule = await _SessionRuleRepository.GetFirstOrDefaultAsync(x => x.Id == id);
            if (SessionRule == null)
            {
                throw new Exception(ErrorMessage.SessionRuleError.SESSION_RULE_NOT_FOUND);
            }
            return SessionRule;
        }

        public async Task<SessionRule> GetSessionRuleByName(string SessionRuleName)
        {
            if (SessionRuleName == null)
            {
                throw new Exception(ErrorMessage.CommonError.NAME_IS_NULL);
            }
            var SessionRule = await _SessionRuleRepository.GetFirstOrDefaultAsync(x => x.Name == SessionRuleName);
            if (SessionRule == null)
            {
                throw new Exception(ErrorMessage.SessionRuleError.SESSION_RULE_NOT_FOUND);
            }
            return SessionRule;
        }

        public async Task<SessionRule> AddNewSessionRule(CreateSessionRuleRequest SessionRuleRequest)
        {

            ValidationResult result = new CreateSessionRuleRequestValidator().Validate(SessionRuleRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            SessionRule SessionRuleCheck = await _SessionRuleRepository.GetFirstOrDefaultAsync(x => x.Name == SessionRuleRequest.Name);

            if (SessionRuleCheck != null)
            {
                throw new Exception(ErrorMessage.SessionRuleError.SESSION_RULE_EXISTED);
            }

            var newSessionRule = new SessionRule();

            newSessionRule.Name = SessionRuleRequest.Name;
            newSessionRule.IncreaseTime = SessionRuleRequest.IncreaseTime;
            newSessionRule.DelayTime = new TimeSpan(
                SessionRuleRequest.DelayTime.days,
                SessionRuleRequest.DelayTime.hours,
                SessionRuleRequest.DelayTime.minutes,
                SessionRuleRequest.DelayTime.seconds,
                milliseconds: 0000001);
            newSessionRule.DelayFreeTime = new TimeSpan(
                SessionRuleRequest.DelayFreeTime.days,
                SessionRuleRequest.DelayFreeTime.hours,
                SessionRuleRequest.DelayFreeTime.minutes,
                SessionRuleRequest.DelayFreeTime.seconds,
                milliseconds: 0000001);
            newSessionRule.FreeTime = new TimeSpan(
                SessionRuleRequest.FreeTime.days,
                SessionRuleRequest.FreeTime.hours,
                SessionRuleRequest.FreeTime.minutes,
                SessionRuleRequest.FreeTime.seconds,
                milliseconds: 0000001);
            newSessionRule.UpdateDate = DateTime.Now;
            newSessionRule.CreateDate = DateTime.Now;
            newSessionRule.Status = true;

            await _SessionRuleRepository.AddAsync(newSessionRule);
            return newSessionRule;
        }

        public async Task<SessionRule> UpdateSessionRule(UpdateSessionRuleRequest SessionRuleRequest)
        {
            try
            {
                var SessionRuleUpdate = await GetSessionRuleByID(SessionRuleRequest.SessionRuleId);

                if (SessionRuleUpdate == null)
                {
                    throw new Exception(ErrorMessage.SessionRuleError.SESSION_RULE_NOT_FOUND);
                }

                ValidationResult result = new UpdateSessionRuleRequestValidator().Validate(SessionRuleRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                SessionRule SessionRuleCheck = _SessionRuleRepository.GetFirstOrDefaultAsync(x => x.Name == SessionRuleRequest.Name).Result;

                if (SessionRuleCheck != null)
                {
                    if(SessionRuleCheck.Id != SessionRuleUpdate.Id)
                    {
                        throw new Exception(ErrorMessage.SessionRuleError.SESSION_RULE_EXISTED);
                    }
                }

                SessionRuleUpdate.Name = SessionRuleRequest.Name;
                SessionRuleUpdate.IncreaseTime = SessionRuleRequest.IncreaseTime;
                SessionRuleUpdate.DelayTime = new TimeSpan(
                SessionRuleRequest.DelayTime.days,
                SessionRuleRequest.DelayTime.hours,
                SessionRuleRequest.DelayTime.minutes,
                SessionRuleRequest.DelayTime.seconds,
                milliseconds: 0000001);
                SessionRuleUpdate.DelayFreeTime = new TimeSpan(
                    SessionRuleRequest.DelayFreeTime.days,
                    SessionRuleRequest.DelayFreeTime.hours,
                    SessionRuleRequest.DelayFreeTime.minutes,
                    SessionRuleRequest.DelayFreeTime.seconds,
                    milliseconds: 0000001);
                SessionRuleUpdate.FreeTime = new TimeSpan(
                    SessionRuleRequest.FreeTime.days,
                    SessionRuleRequest.FreeTime.hours,
                    SessionRuleRequest.FreeTime.minutes,
                    SessionRuleRequest.FreeTime.seconds,
                    milliseconds: 0000001);
                SessionRuleUpdate.UpdateDate = DateTime.Now;
                SessionRuleUpdate.Status = SessionRuleRequest.Status;

                await _SessionRuleRepository.UpdateAsync(SessionRuleUpdate);
                return SessionRuleUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<SessionRule> DeleteSessionRule(Guid SessionRuleDeleteID)
        {
            try
            {
                if (SessionRuleDeleteID == null)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                SessionRule SessionRuleDelete = _SessionRuleRepository.GetFirstOrDefaultAsync(x => x.Id == SessionRuleDeleteID && x.Status == true).Result;

                if (SessionRuleDelete == null)
                {
                    throw new Exception(ErrorMessage.SessionRuleError.SESSION_RULE_NOT_FOUND);
                }

                SessionRuleDelete.Status = false;
                await _SessionRuleRepository.UpdateAsync(SessionRuleDelete);
                return SessionRuleDelete;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at delete type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

    }
}
