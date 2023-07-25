using Business_Logic.Modules.SessionRuleModule.Interface;
using Business_Logic.Modules.SessionRuleModule.Request;
using Business_Logic.Modules.SessionRuleModule.Response;
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

        public async Task<ReturnSessionRule> GetSessionRuleByID(Guid id)
        {
            var returnSession = new ReturnSessionRule();
            if (id == null)
            {
                returnSession.Success = false;
                returnSession.Error = ErrorMessage.CommonError.ID_IS_NULL;
                returnSession.SessionRule = null;
                return returnSession;
            }
            var SessionRule = await _SessionRuleRepository.GetFirstOrDefaultAsync(x => x.Id == id);
            if (SessionRule == null)
            {
                returnSession.Success = false;
                returnSession.Error = ErrorMessage.SessionRuleError.SESSION_RULE_NOT_FOUND;
                returnSession.SessionRule = null;
                return returnSession;
            }
            returnSession.Success = true;
            returnSession.Error = null;
            returnSession.SessionRule = SessionRule;
            return returnSession;
        }

        public async Task<ReturnSessionRule> GetSessionRuleByName(string SessionRuleName)
        {
            var returnSession = new ReturnSessionRule();
            if (SessionRuleName == null)
            {
                returnSession.Success = false;
                returnSession.Error = ErrorMessage.CommonError.NAME_IS_NULL;
                returnSession.SessionRule = null;
            }
            var SessionRule = await _SessionRuleRepository.GetFirstOrDefaultAsync(x => x.Name == SessionRuleName);
            if (SessionRule == null)
            {
                returnSession.Success = false;
                returnSession.Error = ErrorMessage.SessionRuleError.SESSION_RULE_NOT_FOUND;
                returnSession.SessionRule = null;
            }
            returnSession.Success = true;
            returnSession.Error = null;
            returnSession.SessionRule = SessionRule;
            return returnSession;
        }

        public async Task<ReturnSessionRule> AddNewSessionRule(CreateSessionRuleRequest SessionRuleRequest)
        {
            var returnSession = new ReturnSessionRule();
            ValidationResult result = new CreateSessionRuleRequestValidator().Validate(SessionRuleRequest);
            if (!result.IsValid)
            {
                returnSession.Success = false;
                returnSession.Error = ErrorMessage.CommonError.INVALID_REQUEST;
                returnSession.SessionRule = null;
                return returnSession;
            }

            SessionRule SessionRuleCheck = await _SessionRuleRepository.GetFirstOrDefaultAsync(x => x.Name == SessionRuleRequest.Name);

            if (SessionRuleCheck != null)
            {
                returnSession.Success = false;
                returnSession.Error = ErrorMessage.SessionRuleError.SESSION_RULE_EXISTED;
                returnSession.SessionRule = null;
                return returnSession;
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
            returnSession.Success = true;
            returnSession.Error = null;
            returnSession.SessionRule = newSessionRule;
            return returnSession;
        }

        public async Task<ReturnSessionRule> UpdateSessionRule(UpdateSessionRuleRequest SessionRuleRequest)
        {
            try
            {
                var returnSession = new ReturnSessionRule();
                var SessionRuleUpdate = await _SessionRuleRepository.GetFirstOrDefaultAsync(s => s.Id == SessionRuleRequest.SessionRuleId);

                if (SessionRuleUpdate == null)
                {
                    returnSession.Success = false;
                    returnSession.Error = ErrorMessage.SessionRuleError.SESSION_RULE_NOT_FOUND;
                    returnSession.SessionRule = null;
                    return returnSession;
                }

                ValidationResult result = new UpdateSessionRuleRequestValidator().Validate(SessionRuleRequest);
                if (!result.IsValid)
                {
                    returnSession.Success = false;
                    returnSession.Error = ErrorMessage.CommonError.INVALID_REQUEST;
                    returnSession.SessionRule = null;
                    return returnSession;
                }

                SessionRule SessionRuleCheck = _SessionRuleRepository.GetFirstOrDefaultAsync(x => x.Name == SessionRuleRequest.Name).Result;

                if (SessionRuleCheck != null)
                {
                    if(SessionRuleCheck.Id != SessionRuleUpdate.Id)
                    {
                        returnSession.Success = false;
                        returnSession.Error = ErrorMessage.SessionRuleError.SESSION_RULE_EXISTED;
                        returnSession.SessionRule = null;
                        return returnSession;
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
                returnSession.Success = true;
                returnSession.Error = null;
                returnSession.SessionRule = SessionRuleUpdate;
                return returnSession;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ReturnSessionRule> DeleteSessionRule(Guid SessionRuleDeleteID)
        {
            try
            {
                var returnSession = new ReturnSessionRule();
                if (SessionRuleDeleteID == null)
                {
                    returnSession.Success = false;
                    returnSession.Error = ErrorMessage.CommonError.ID_IS_NULL;
                    returnSession.SessionRule = null;
                    return returnSession;
                }

                SessionRule SessionRuleDelete = _SessionRuleRepository.GetFirstOrDefaultAsync(x => x.Id == SessionRuleDeleteID && x.Status == true).Result;

                if (SessionRuleDelete == null)
                {
                    returnSession.Success = false;
                    returnSession.Error = ErrorMessage.SessionRuleError.SESSION_RULE_NOT_FOUND;
                    returnSession.SessionRule = null;
                    return returnSession;
                }

                SessionRuleDelete.Status = false;
                await _SessionRuleRepository.UpdateAsync(SessionRuleDelete);
                returnSession.Success = true;
                returnSession.Error = null;
                returnSession.SessionRule = SessionRuleDelete;
                return returnSession;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at delete type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

    }
}
