using System;
using System.Threading.Tasks;
using TeacherPractise.Model;

namespace TeacherPractise.Service.Token.RegistrationToken
{
    public class ConfirmationTokenService
    {
        public void saveConfirmationToken(ConfirmationToken token)
        {
            using (var ctx = new Context())
	        {
                ctx.ConfirmationTokens.Add(token);
                ctx.SaveChanges();
            }
        }

        public ConfirmationToken? getToken(string token)
        {
            using (var ctx = new Context())
	        {
                ConfirmationToken confToken = ctx.ConfirmationTokens.Where(q => q.Token == token.ToLower()).FirstOrDefault();
                if (confToken != null) return confToken;
                else return null;
            }
        }

        public void deleteByExpiresAtLessThan(DateTime now) //nastavit do program.cs jako cron
        {
            using (var ctx = new Context())
	        {
                var confTokens = ctx.ConfirmationTokens.ToList();
                foreach (var confToken in confTokens)
                {
                    if (confToken.ExpiresAt < now) ctx.ConfirmationTokens.Remove(confToken);
                }
            }
        }

        public List<ConfirmationToken> findAll()
        {
            using (var ctx = new Context())
	        {
                var confTokens = ctx.ConfirmationTokens.ToList();
                return confTokens;
            }
        }

        public int setConfirmedAt(string token)
        {
            using (var ctx = new Context())
	        {
                ConfirmationToken confToken = ctx.ConfirmationTokens.Where(q => q.Token == token.ToLower()).FirstOrDefault();
                confToken.ConfirmedAt = DateTime.Now;
                return ctx.SaveChanges();
            }
        }

        public int deleteConfirmationTokenById(long userId)
        {
            using (var ctx = new Context())
	        {
                ConfirmationToken confToken = ctx.ConfirmationTokens.Where(q => q.AppUser.Id == (int)userId).FirstOrDefault();
                ctx.ConfirmationTokens.Remove(confToken);
                return ctx.SaveChanges();
            }
        }
    }
}