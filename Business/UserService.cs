using System;
using System.Collections.Generic;
using System.Linq;
using UsersAPI.Data;
using UsersAPI.Models;

namespace UsersAPI.Business
{
    public class UserService
    {
        private UsersDBContext _context;

        public UserService(UsersDBContext context)
        {
            _context = context;
        }

        public User Retrieve(int id)
        {
            return _context.Users.Where(
                    u => u.Id == id).FirstOrDefault();
            
        }

        public IEnumerable<User> ListAll()
        {
            return _context.Users
                .OrderBy(u => u.Name).ToList();
        }

        public Result Add(User userData)
        {
            Result resultado = ValidateData(userData);
            resultado.Action = "Inclusão de Usuário";

            if (resultado.Inconsistencies.Count == 0 &&
                _context.Users.Where(
                u => u.Email == userData.Email).Count() > 0)
            {
                resultado.Inconsistencies.Add(
                    "Usuário já cadastrado");
            }

            if (resultado.Inconsistencies.Count == 0)
            {
                _context.Users.Add(userData);
                _context.SaveChanges();
            }

            return resultado;
        }

        public Result Update(User userData)
        {
            Result result = ValidateData(userData);
            result.Action = "Atualização de Usuário";

            if (result.Inconsistencies.Count == 0)
            {
                User user = _context.Users.Where(
                    u => u.Id == userData.Id).FirstOrDefault();

                if (user == null)
                {
                    result.Inconsistencies.Add(
                        "Usuário não encontrado");
                }
                else
                {
                    user.Name = userData.Name;
                    user.Email = userData.Email;
                    _context.SaveChanges();
                }
            }

            return result;
        }

        public Result Delete(int id)
        {
            Result result = new Result();
            result.Action = "Exclusão de Usuário";

            User user = Retrieve(id);
            if (user == null)
            {
                result.Inconsistencies.Add(
                    "Usuário não encontrado");
            }
            else
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
                
            return result;
        }

        private Result ValidateData(User user)
        {
            var result = new Result();
            if (user == null)
            {
                result.Inconsistencies.Add(
                    "Preencha os dados do Usuário");
            }
            else
            {
                if (String.IsNullOrWhiteSpace(user.Name))
                {
                    result.Inconsistencies.Add(
                        "Preencha o Nome do Usuário");
                }

                if (String.IsNullOrWhiteSpace(user.Email))
                {
                    result.Inconsistencies.Add(
                        "Preencha o Email do Usuário");
                }
            }

            return result;
        }
    }
}