using System;
using System.Collections.Generic;
using System.Linq;
using UsersAPI.Data;
using UsersAPI.Models;

namespace UsersAPI.Business
{
    public class RatingService
    {
        private RatingsDBContext _context;
        private enum Type {Playlist, Music, Artist};

        public RatingService(RatingsDBContext context)
        {
            _context = context;
        }

        public Rating Retrieve(int id)
        {
            return _context.Ratings.Where(
                    r => r.RatingId == id).FirstOrDefault();
            
        }

        public IEnumerable<Rating> ListAll()
        {
            return _context.Ratings
                .OrderBy(r => r.RatingId).ToList();
        }

        public Result Add(Rating ratingData)
        {
            Result resultado = ValidateData(ratingData);
            resultado.Action = "Inclusão de Reputação";

            if (resultado.Inconsistencies.Count == 0 &&
                _context.Ratings.Where(
                r => r.ArtistId == ratingData.ArtistId || 
                r.MusicId == ratingData.MusicId ||
                r.PlaylistId == r.PlaylistId)
                .Count() > 0)
            {
                resultado.Inconsistencies.Add(
                    "Reputação já cadastrada");
            }

            if (resultado.Inconsistencies.Count == 0)
            {
                _context.Ratings.Add(ratingData);
                _context.SaveChanges();
            }

            return resultado;
        }

        public Result Update(Rating ratingData)
        {
            Result result = ValidateData(ratingData);
            result.Action = "Atualização de Reputação";

            if (result.Inconsistencies.Count == 0)
            {
                Rating rating = _context.Ratings.Where(
                    r => r.RatingId == ratingData.RatingId).FirstOrDefault();

                if (rating == null)
                {
                    result.Inconsistencies.Add(
                        "Usuário não encontrado");
                }
                else
                {
                    rating.ArtistId = ratingData.ArtistId;
                    rating.MusicId = ratingData.MusicId;
                    rating.PlaylistId = ratingData.PlaylistId;
                    rating.UserId = ratingData.UserId;
                    rating.Score = ratingData.Score;
                    rating.Type = ratingData.Type;
                    rating.Comment = ratingData.Comment;
                    _context.SaveChanges();
                }
            }

            return result;
        }

        public Result Delete(int id)
        {
            Result result = new Result();
            result.Action = "Exclusão de Usuário";

            Rating rating = Retrieve(id);
            if (rating == null)
            {
                result.Inconsistencies.Add(
                    "Usuário não encontrado");
            }
            else
            {
                _context.Ratings.Remove(rating);
                _context.SaveChanges();
            }
                
            return result;
        }

        private Result ValidateData(Rating rating)
        {
            var result = new Result();
            if (rating == null)
            {
                result.Inconsistencies.Add(
                    "Preencha os dados do Usuário");
            }
            else
            {
                if (String.IsNullOrWhiteSpace(rating.Type))
                {
                    result.Inconsistencies.Add(
                        "Preencha o que está sendo avaliado");
                }

                if(!Enum.GetNames(typeof(Type)).Contains(rating.Type))
                {
                    result.Inconsistencies.Add(
                        "Tipo de avaliação errado");
                } else {
                    if(Enum.GetNames(typeof(Type))[0].Equals(rating.Type))
                    {
                        if(rating.PlaylistId==0)
                            result.Inconsistencies.Add("Playlist avaliada deve ser fornecida");
                    } else if(Enum.GetNames(typeof(Type))[1].Equals(rating.Type))
                    {
                        if(rating.MusicId==0)
                            result.Inconsistencies.Add("Música avaliada deve ser fornecida");
                    } else if(Enum.GetNames(typeof(Type))[2].Equals(rating.Type))
                    {
                        if(rating.ArtistId==0)
                            result.Inconsistencies.Add("Artista avaliado deve ser fornecida");
                    }
                }

                if (rating.Score==0)
                {
                    result.Inconsistencies.Add(
                        "Preencha a Avaliação");
                }

            }

            return result;
        }
    }
}