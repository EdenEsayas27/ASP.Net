using MongoDB.Driver;
using Tvee.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tvee.Services
{
    public class QAService
    {
        private readonly IMongoCollection<Question> _questions;

        public QAService(IMongoDatabase database)
        {
            _questions = database.GetCollection<Question>("Questions");
        }

        // Post a new question
        public async Task<Question> PostQuestion(Question question)
        {
            await _questions.InsertOneAsync(question);
            return question;
        }

        // Post an answer to a question
        public async Task<Question?> PostAnswer(string questionId, Answer answer)
        {
            var filter = Builders<Question>.Filter.Eq(q => q.Id, questionId);
            var question = await _questions.Find(filter).FirstOrDefaultAsync();

            if (question == null)
                return null; // Question doesn't exist

            // Ensure answer doesn't already exist
            if (question.Answers.Exists(a => a.UserId == answer.UserId && a.Content == answer.Content))
                return question;

            var update = Builders<Question>.Update.Push(q => q.Answers, answer);
            await _questions.UpdateOneAsync(filter, update);
            return await _questions.Find(filter).FirstOrDefaultAsync();
        }

        // Get all questions
        public async Task<List<Question>> GetAllQuestions()
        {
            return await _questions.Find(_ => true)
                .Sort(Builders<Question>.Sort.Descending(q => q.Id)) // Sorting by Id if Timestamp is removed
                .ToListAsync();
        }

        // Get a question by ID
        public async Task<Question?> GetQuestionById(string questionId)
        {
            return await _questions.Find(q => q.Id == questionId).FirstOrDefaultAsync();
        }

        // Edit a question (only the user who posted it can edit)
        public async Task<Question?> EditQuestion(string questionId, string userId, string newContent)
        {
            var filter = Builders<Question>.Filter.And(
                Builders<Question>.Filter.Eq(q => q.Id, questionId),
                Builders<Question>.Filter.Eq(q => q.UserId, userId)
            );

            var update = Builders<Question>.Update.Set(q => q.Content, newContent);
            var result = await _questions.UpdateOneAsync(filter, update);

            if (result.ModifiedCount == 0)
                return null; // Return null if the question was not found or the user is unauthorized

            return await _questions.Find(filter).FirstOrDefaultAsync();
        }

        // Delete a question (only the user who posted it can delete)
        public async Task<bool> DeleteQuestion(string questionId, string userId)
        {
            var filter = Builders<Question>.Filter.And(
                Builders<Question>.Filter.Eq(q => q.Id, questionId),
                Builders<Question>.Filter.Eq(q => q.UserId, userId)
            ); // Ensure only the owner can delete

            var result = await _questions.DeleteOneAsync(filter);
            return result.DeletedCount > 0; // Return true if a document was deleted
        }
    }
}