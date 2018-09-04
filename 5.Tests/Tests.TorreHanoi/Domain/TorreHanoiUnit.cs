using System;
using Domain.TorreHanoi;
using Infrastructure.TorreHanoi.Log;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests.TorreHanoi.Domain
{
    [TestClass]
    public class TorreHanoiUnit
    {
        private const string CategoriaTeste = "Domain/TorreHanoi";

        private Mock<ILogger> _mockLogger;
        private global::Domain.TorreHanoi.TorreHanoi _torreHanoi;

        [TestInitialize]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger>();
            _mockLogger.Setup(s => s.Logar(It.IsAny<string>(), It.IsAny<TipoLog>()));
        }

        [TestMethod]
        [TestCategory(CategoriaTeste)]
        public void Construtor_DeveInicializarObjetoComOsValoresEsperados()
        {
            _torreHanoi = new global::Domain.TorreHanoi.TorreHanoi(3, _mockLogger.Object);

            Assert.AreNotEqual(_torreHanoi.Id, Guid.Empty);
            Assert.AreEqual(_torreHanoi.Status, TipoStatus.Pendente);
            Assert.AreEqual(_torreHanoi.Discos.Count, 3);
            Assert.IsTrue(DateTime.Now > _torreHanoi.DataCriacao);
        }

        [TestMethod]
        [TestCategory(CategoriaTeste)]
        public void Processar_Deverar_Retornar_Sucesso()
        {
            _torreHanoi = new global::Domain.TorreHanoi.TorreHanoi(4, _mockLogger.Object);

            _torreHanoi.Processar();

            Assert.AreEqual(_torreHanoi.PassoAPasso.Count, 15);
            Assert.AreEqual(_torreHanoi.Destino.Discos.Count, 4);
        }
    }
}
