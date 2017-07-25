using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZQuiz.DataModel.GenericRepository;

namespace ZQuiz.DataModel.UnitOfWork
{
    /// <summary>
    /// Unit of Work class responsible for DB transactions
    /// </summary>
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly ZQuiz3DBEntities _context = null;
        GenericRespository<Tester> _testerRespository;
        GenericRespository<Question> _questionRepository;
        GenericRespository<Choice> _choiceRepository;
        GenericRespository<TesterQuestion> _testerQuestionRepository;

        public UnitOfWork()
        {
            _context = new ZQuiz3DBEntities();
        }

        /// <summary>
        /// Get/Set Property for tester repository.
        /// </summary>
        public GenericRespository<Tester> TesterRespository
        {
            get
            {
                if (this._testerRespository == null)
                    this._testerRespository = new GenericRespository<Tester>(_context);
                return this._testerRespository;
            }
        }

        /// <summary>
        /// Get/Set Property for question repository.
        /// </summary>
        public GenericRespository<Question> QuestionRepository
        {
            get
            {
                if (this._questionRepository == null)
                    this._questionRepository = new GenericRespository<Question>(_context);
                return this._questionRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for choice repository.
        /// </summary>
        public GenericRespository<Choice> ChoiceRepository
        {
            get
            {
                if (this._choiceRepository == null)
                    this._choiceRepository = new GenericRespository<Choice>(_context);
                return this._choiceRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for tester questions repository.
        /// </summary>
        public GenericRespository<TesterQuestion> TesterQuestionRepository
        {
            get
            {
                if (this._testerQuestionRepository == null)
                    this._testerQuestionRepository = new GenericRespository<TesterQuestion>(_context);
                return this._testerQuestionRepository;
            }
        }

        /// <summary>
        /// Save method.
        /// </summary>
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw e;
            }

        }

        private bool disposed = false;

        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
