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

        public void deleteExpiredConfirmationTokens()
        {
            using (var ctx = new Context())
	        {
                var confTokens = ctx.ConfirmationTokens.ToList();
                foreach (var confToken in confTokens)
                {
                    if (confToken.ExpiresAt < DateTime.Now) 
                    {
                        ctx.ConfirmationTokens.Remove(confToken);
                        ctx.SaveChanges();
                    }
                }
            }
        }

        public void deleteExpiredPasswordResetTokens()
        {
            using (var ctx = new Context())
	        {
                var resetTokens = ctx.PasswordResetTokens.ToList();
                foreach (var resetToken in resetTokens)
                {
                    if (resetToken.ExpiryDate < DateTime.Now) 
                    {
                        ctx.PasswordResetTokens.Remove(resetToken);
                        ctx.SaveChanges();
                    }
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
                ConfirmationToken confToken = ctx.ConfirmationTokens.Where(q => q.ConfirmationTokenId == (int)userId).FirstOrDefault();
                ctx.ConfirmationTokens.Remove(confToken);
                return ctx.SaveChanges();
            }
        }
    }
}