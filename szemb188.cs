using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;
using System.Text;
using System.Globalization;
using System.Diagnostics;
using System.Text.RegularExpressions;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;
using System.IO;
using Microsoft.Extensions.Logging;
using com.caixa.backend.Utils;
using Ionate.Utils;
using Ionate.Cobol;
using com.caixa.backend.Models;
using static com.caixa.backend.Utils.AppUtils;
using com.caixa.copybook.Objects.SZ057;
using com.caixa.backend.Objects.Szemb188;
using com.caixa.copybook.Objects.SQLCA;
using com.caixa.copybook.Objects.SZ053;
using com.caixa.copybook.Objects.SZEMWL01;
using com.caixa.copybook.Objects.SZ115;
using com.caixa.copybook.Objects.SZ010;
using com.caixa.copybook.Objects.SZ063;
using com.caixa.copybook.Objects.SZ008;
using com.caixa.copybook.Objects.SZ073;
using com.caixa.copybook.Objects.SZ077;
using com.caixa.copybook.Objects.SZ021;
using com.caixa.copybook.Objects.SZ072;
using com.caixa.copybook.Objects.SZ012;
using com.caixa.copybook.Objects.SZ098;
using com.caixa.copybook.Objects.SZ011;
using com.caixa.copybook.Objects.SZ043;
using com.caixa.copybook.Objects.CBO;

// Author: DIOGO MATHEUS.
// Date Written: 08/11/2022.
// *****************************************************************
// SMART - SISTEMA DE ADMINISTRACAO DE CONTRATOS DE SEGURO DE     *
// CREDITO FINANCEIRO                                     *
// ----------------------------------------------------------------*
// PROGRAMA : SZEMB188.                                           *
// DESCRICAO: GERA ARQUIVO ACOPLADO SAF (TEMPO ASSIST) MENSAL     *
// ----------------------------------------------------------------*
// HISTORICO DE CORRECOES                                         *
// ----------------------------------------------------------------*
// NO VERSAO DATA       RESPONSAVEL      ALTERACAO                *
// ----------------------------------------------------------------*
// ----------------------------------------------------------------*
// VERSAO  : 002
// MOTIVO  : AJUSTE NO ENVIO DT INICIO/FIM DE VIGENCIA DO CONTRATO
// JAZZ    : 441629
// DATA    : 20/12/2022
// NOME    : DIOGO MATHEUS
// MARCADOR: V.2
// ----------------------------------------------------------------*
// ----------------------------------------------------------------*
// VERSAO  : 001
// MOTIVO  : ARQUIVO SAF (TEMPO ASSIST) MENSAL
// JAZZ    : 441629
// DATA    : 08/11/2022
// NOME    : DIOGO MATHEUS
// MARCADOR: V.1
// ----------------------------------------------------------------*
namespace com.caixa.backend
{
    public class Szemb188
    {
        public Szemb188()
        {
        }

        static Szemb188()
        {
            DataTypeMap.Add("RegArqtemp1", new DataTypeMetadata("X(520)", DataTypeMetadata.DataType.STRING, 520, 0L, 520L));
            DataTypeMap.Add("WPrograma", new DataTypeMetadata("X(008)", DataTypeMetadata.DataType.STRING, 8, 520L, 528L));
            DataTypeMap.Add("WReturnCode", new DataTypeMetadata("9(006)", DataTypeMetadata.DataType.INTEGER, 6, 6, 0, 528L, 534L, null));
            DataTypeMap.Add("WProgramaVersao", new DataTypeMetadata("X(080)", DataTypeMetadata.DataType.STRING, 80, 534L, 614L));
            DataTypeMap.Add("WLabel", new DataTypeMetadata("X(005)", DataTypeMetadata.DataType.STRING, 5, 614L, 619L));
            DataTypeMap.Add("WCall", new DataTypeMetadata("X(008)", DataTypeMetadata.DataType.STRING, 8, 619L, 627L));
            DataTypeMap.Add("WOpenArq", new DataTypeMetadata("X(003)", DataTypeMetadata.DataType.STRING, 3, 627L, 630L));
            DataTypeMap.Add("WFimCertif", new DataTypeMetadata("X(003)", DataTypeMetadata.DataType.STRING, 3, 630L, 633L));
            DataTypeMap.Add("WFimPactuante", new DataTypeMetadata("X(003)", DataTypeMetadata.DataType.STRING, 3, 633L, 636L));
            DataTypeMap.Add("WAtualObjAcop", new DataTypeMetadata("9(009)", DataTypeMetadata.DataType.INTEGER, 9, 9, 0, 636L, 645L, null));
            DataTypeMap.Add("WAtualObjAcopAss", new DataTypeMetadata("9(009)", DataTypeMetadata.DataType.INTEGER, 9, 9, 0, 645L, 654L, null));
            DataTypeMap.Add("WSzemb188Trace", new DataTypeMetadata("X(008)", DataTypeMetadata.DataType.STRING, 8, 654L, 662L));
            DataTypeMap.Add("WSeqRegistro", new DataTypeMetadata("9(009)", DataTypeMetadata.DataType.INTEGER, 9, 9, 0, 662L, 671L, null));
            DataTypeMap.Add("WMensagemErro", new DataTypeMetadata("X(500)", DataTypeMetadata.DataType.STRING, 500, 671L, 1171L));
            DataTypeMap.Add("WQtdDisplayTrace", new DataTypeMetadata("9(008)", DataTypeMetadata.DataType.INTEGER, 8, 8, 0, 1171L, 1179L, null));
            DataTypeMap.Add("WQtdDisplayLog", new DataTypeMetadata("9(008)", DataTypeMetadata.DataType.INTEGER, 8, 8, 0, 1179L, 1187L, null));
            DataTypeMap.Add("WIntegerValorMax", new DataTypeMetadata("S9(09)", DataTypeMetadata.DataType.INTEGER, 9, 9, 0, 1187L, 1196L, null));
            DataTypeMap.Add("WCodPessoa", new DataTypeMetadata("9(010)", DataTypeMetadata.DataType.INTEGER, 10, 10, 0, 1196L, 1206L, null));
            DataTypeMap.Add("WNumContratoTerc", new DataTypeMetadata("9(018)", DataTypeMetadata.DataType.INTEGER, 18, 18, 0, 1206L, 1224L, null));
            DataTypeMap.Add("WNumContratoAnt", new DataTypeMetadata("9(018)", DataTypeMetadata.DataType.INTEGER, 18, 18, 0, 1224L, 1242L, null));
            DataTypeMap.Add("WNumContrApolice", new DataTypeMetadata("9(018)", DataTypeMetadata.DataType.INTEGER, 18, 18, 0, 1242L, 1260L, null));
            DataTypeMap.Add("WNumContrato", new DataTypeMetadata("9(009)", DataTypeMetadata.DataType.INTEGER, 9, 9, 0, 1260L, 1269L, null));
            DataTypeMap.Add("WNumOcupacao", new DataTypeMetadata("9(009)", DataTypeMetadata.DataType.INTEGER, 9, 9, 0, 1269L, 1278L, null));
            DataTypeMap.Add("WDdi", new DataTypeMetadata("S9(004)", DataTypeMetadata.DataType.INTEGER, 4, 4, 0, 1278L, 1282L, null));
            DataTypeMap.Add("WTotGravados", new DataTypeMetadata("9(006)", DataTypeMetadata.DataType.INTEGER, 6, 6, 0, 1282L, 1288L, null));
            DataTypeMap.Add("WTotLidosCertComp", new DataTypeMetadata("9(008)", DataTypeMetadata.DataType.INTEGER, 8, 8, 0, 1288L, 1296L, null));
            DataTypeMap.Add("WTotLidosCertPact", new DataTypeMetadata("9(008)", DataTypeMetadata.DataType.INTEGER, 8, 8, 0, 1296L, 1304L, null));
            DataTypeMap.Add("WQtdCompra1", new DataTypeMetadata("9(008)", DataTypeMetadata.DataType.INTEGER, 8, 8, 0, 1304L, 1312L, null));
            DataTypeMap.Add("WQtdCompra2", new DataTypeMetadata("9(008)", DataTypeMetadata.DataType.INTEGER, 8, 8, 0, 1312L, 1320L, null));
            DataTypeMap.Add("WQtdCancel", new DataTypeMetadata("9(008)", DataTypeMetadata.DataType.INTEGER, 8, 8, 0, 1320L, 1328L, null));
            DataTypeMap.Add("WsDtRef", new DataTypeMetadata("X(010)", DataTypeMetadata.DataType.STRING, 10, 1328L, 1338L));
            DataTypeMap.Add("WsDtIni", new DataTypeMetadata("X(010)", DataTypeMetadata.DataType.STRING, 10, 1338L, 1348L));
            DataTypeMap.Add("WsDtFim", new DataTypeMetadata("X(010)", DataTypeMetadata.DataType.STRING, 10, 1348L, 1358L));
            DataTypeMap.Add("ESqlcode", new DataTypeMetadata("Z999-", DataTypeMetadata.DataType.INTEGER, 4, 4, 0, 1358L, 1362L, null));
            DataTypeMap.Add("ESmallint1", new DataTypeMetadata("-----9", DataTypeMetadata.DataType.INTEGER, 6, 6, 0, 1362L, 1368L, null));
            DataTypeMap.Add("EInteger1", new DataTypeMetadata("----------9", DataTypeMetadata.DataType.INTEGER, 11, 11, 0, 1368L, 1379L, null));
            DataTypeMap.Add("EInteger2", new DataTypeMetadata("----------9", DataTypeMetadata.DataType.INTEGER, 11, 11, 0, 1379L, 1390L, null));
            DataTypeMap.Add("EInteger3", new DataTypeMetadata("----------9", DataTypeMetadata.DataType.INTEGER, 11, 11, 0, 1390L, 1401L, null));
            DataTypeMap.Add("EInteger4", new DataTypeMetadata("----------9", DataTypeMetadata.DataType.INTEGER, 11, 11, 0, 1401L, 1412L, null));
            DataTypeMap.Add("EBigint1", new DataTypeMetadata("9(018)", DataTypeMetadata.DataType.INTEGER, 18, 18, 0, 1412L, 1430L, null));
            DataTypeMap.Add("HDtaCurrent", new DataTypeMetadata("X(010)", DataTypeMetadata.DataType.STRING, 10, 1430L, 1440L));
            DataTypeMap.Add("HNumVersaoSerie", new DataTypeMetadata("S9(004)", DataTypeMetadata.DataType.INTEGER, 4, 4, 0, 1440L, 1444L, null));
            DataTypeMap.Add("HCodProdutoMin", new DataTypeMetadata("S9(09)", DataTypeMetadata.DataType.INTEGER, 9, 9, 0, 1444L, 1453L, null));
            DataTypeMap.Add("HCodProdutoMax", new DataTypeMetadata("S9(09)", DataTypeMetadata.DataType.INTEGER, 9, 9, 0, 1453L, 1462L, null));
            DataTypeMap.Add("HCodAcopladoMin", new DataTypeMetadata("S9(09)", DataTypeMetadata.DataType.INTEGER, 9, 9, 0, 1462L, 1471L, null));
            DataTypeMap.Add("HCodAcopladoMax", new DataTypeMetadata("S9(09)", DataTypeMetadata.DataType.INTEGER, 9, 9, 0, 1471L, 1480L, null));
            DataTypeMap.Add("HDtaIniVigenciaAtual", new DataTypeMetadata("X(010)", DataTypeMetadata.DataType.STRING, 10, 1480L, 1490L));
            DataTypeMap.Add("HDtaIniVigenciaOrig", new DataTypeMetadata("X(010)", DataTypeMetadata.DataType.STRING, 10, 1490L, 1500L));
            DataTypeMap.Add("HDtaFimCarenciaOrig", new DataTypeMetadata("X(010)", DataTypeMetadata.DataType.STRING, 10, 1500L, 1510L));
            DataTypeMap.Add("NSz012NumContratoAnt", new DataTypeMetadata("S9(004)", DataTypeMetadata.DataType.INTEGER, 4, 4, 0, 1510L, 1514L, null));
            DataTypeMap.Add("Ftm01HRegHeader", new DataTypeMetadata(null, DataTypeMetadata.DataType.OBJECT, 520, 1514L, 2034L));
            DataTypeMap.Add("Ftm01DRegDetalhe", new DataTypeMetadata(null, DataTypeMetadata.DataType.OBJECT, 520, 2034L, 2554L));
            DataTypeMap.Add("Ftm01TRegTrailler", new DataTypeMetadata(null, DataTypeMetadata.DataType.OBJECT, 520, 2554L, 3074L));
            DataTypeMap.Add("Szemb188Parametros", new DataTypeMetadata(null, DataTypeMetadata.DataType.OBJECT, 24, 3074L, 3098L));
            DataTypeMap.Add("DclszPessoa", new DataTypeMetadata(null, DataTypeMetadata.DataType.OBJECT, 157, 3098L, 3255L));
            DataTypeMap.Add("DclszApolice", new DataTypeMetadata(null, DataTypeMetadata.DataType.OBJECT, 263, 3255L, 3518L));
            DataTypeMap.Add("DclszOrigemContrato", new DataTypeMetadata(null, DataTypeMetadata.DataType.OBJECT, 80, 3518L, 3598L));
            DataTypeMap.Add("DclszContrTerc", new DataTypeMetadata(null, DataTypeMetadata.DataType.OBJECT, 190, 3598L, 3788L));
            DataTypeMap.Add("DclszContrSeguro", new DataTypeMetadata(null, DataTypeMetadata.DataType.OBJECT, 181, 3788L, 3969L));
            DataTypeMap.Add("DclszObjAcopladoAssist", new DataTypeMetadata(null, DataTypeMetadata.DataType.OBJECT, 68, 3969L, 4037L));
            DataTypeMap.Add("DclszPessoaFisica", new DataTypeMetadata(null, DataTypeMetadata.DataType.OBJECT, 69, 4037L, 4106L));
            DataTypeMap.Add("DclszPessoaTelefone", new DataTypeMetadata(null, DataTypeMetadata.DataType.OBJECT, 44, 4106L, 4150L));
            DataTypeMap.Add("DclszEndereco", new DataTypeMetadata(null, DataTypeMetadata.DataType.OBJECT, 527, 4150L, 4677L));
            DataTypeMap.Add("DclszAcoplado", new DataTypeMetadata(null, DataTypeMetadata.DataType.OBJECT, 384, 4677L, 5061L));
            DataTypeMap.Add("DclszApolAcoplado", new DataTypeMetadata(null, DataTypeMetadata.DataType.OBJECT, 91, 5061L, 5152L));
            DataTypeMap.Add("DclszObjEndereco", new DataTypeMetadata(null, DataTypeMetadata.DataType.OBJECT, 77, 5152L, 5229L));
            DataTypeMap.Add("DclszAcopladoAssist", new DataTypeMetadata(null, DataTypeMetadata.DataType.OBJECT, 113, 5229L, 5342L));
            DataTypeMap.Add("DclszObjAcoplado", new DataTypeMetadata(null, DataTypeMetadata.DataType.OBJECT, 86, 5342L, 5428L));
            DataTypeMap.Add("Dclcbo", new DataTypeMetadata(null, DataTypeMetadata.DataType.OBJECT, 74, 5428L, 5502L));
            DataTypeMap.Add("Sqlca", new DataTypeMetadata(null, DataTypeMetadata.DataType.OBJECT, 136, 5502L, 5638L));
            DataTypeMap.Add("HSzl01Szemnl01", new DataTypeMetadata(null, DataTypeMetadata.DataType.OBJECT, 1002, 5638L, 6640L));
            DataTypeMap.Add("NSzl01Szemnl01", new DataTypeMetadata(null, DataTypeMetadata.DataType.OBJECT, 52, 6640L, 6692L));
            DataTypeMap.Add("WGe3000BProgCall", new DataTypeMetadata("X(008)", DataTypeMetadata.DataType.STRING, 8, 6692L, 6700L));
            DataTypeMap.Add("WGe3000BParagrafoOrig", new DataTypeMetadata("X(005)", DataTypeMetadata.DataType.STRING, 5, 6700L, 6705L));
            DataTypeMap.Add("WGe3000BReturnCode", new DataTypeMetadata("9(009)", DataTypeMetadata.DataType.INTEGER, 9, 9, 0, 6705L, 6714L, null));
            DataTypeMap.Add("WGe3000BFimArq", new DataTypeMetadata("X(003)", DataTypeMetadata.DataType.STRING, 3, 6714L, 6717L));
            DataTypeMap.Add("WGe3000BCount", new DataTypeMetadata("9(010)", DataTypeMetadata.DataType.INTEGER, 10, 10, 0, 6717L, 6727L, null));
            DataTypeMap.Add("WGe3000BIndErroIni", new DataTypeMetadata("9(004)", DataTypeMetadata.DataType.INTEGER, 4, 4, 0, 6727L, 6731L, null));
            DataTypeMap.Add("WGe3000BIndErroGra", new DataTypeMetadata("9(004)", DataTypeMetadata.DataType.INTEGER, 4, 4, 0, 6731L, 6735L, null));
            DataTypeMap.Add("WGe3000BIndErroFin", new DataTypeMetadata("9(004)", DataTypeMetadata.DataType.INTEGER, 4, 4, 0, 6735L, 6739L, null));
            DataTypeMap.Add("WGe3000BMsgErroIni", new DataTypeMetadata("X(120)", DataTypeMetadata.DataType.STRING, 120, 6739L, 6859L));
            DataTypeMap.Add("WGe3000BMsgErroGra", new DataTypeMetadata("X(120)", DataTypeMetadata.DataType.STRING, 120, 6859L, 6979L));
            DataTypeMap.Add("WGe3000BMsgErroFin", new DataTypeMetadata("X(120)", DataTypeMetadata.DataType.STRING, 120, 6979L, 7099L));
            DataTypeMap.Add("WGe3000BMsgErro", new DataTypeMetadata("X(120)", DataTypeMetadata.DataType.STRING, 120, 7099L, 7219L));
            DataTypeMap.Add("WGe3000BCtTrace", new DataTypeMetadata("X(003)", DataTypeMetadata.DataType.STRING, 3, 7219L, 7222L));
            DataTypeMap.Add("LkGe3000BParametros", new DataTypeMetadata(null, DataTypeMetadata.DataType.OBJECT, 2216, 7222L, 9438L));
            DataTypeMap.Add("Szemb188FileParametros", new DataTypeMetadata(null, DataTypeMetadata.DataType.OBJECT, 45, 9438L, 9483L));
        }

        public string RegArqtemp1 { get; set; } = Utility.PadLeft("", 520, "\0");


        public string WPrograma { get; set; } = "SZEMB188";


        public long WReturnCode { get; set; } = 0L;


        public string WProgramaVersao { get; set; } = Utility.PadLeft("", 80, " ");


        public string WLabel { get; set; } = Utility.PadLeft("", 5, " ");


        public string WCall { get; set; } = Utility.PadLeft("", 8, " ");


        public string WOpenArq { get; set; } = "NAO";


        public string WFimCertif { get; set; } = Utility.PadLeft("", 3, " ");


        public string WFimPactuante { get; set; } = Utility.PadLeft("", 3, " ");


        public long WAtualObjAcop { get; set; } = 0L;


        public long WAtualObjAcopAss { get; set; } = 0L;


        public string WSzemb188Trace { get; set; } = Utility.PadLeft("", 8, "\0");


        public long WSeqRegistro { get; set; } = 0L;


        public string WMensagemErro { get; set; } = Utility.PadLeft("", 500, " ");


        public long WQtdDisplayTrace { get; set; } = 0L;


        public long WQtdDisplayLog { get; set; } = 0L;


        public long WIntegerValorMax { get; set; } = 0L;


        public long WCodPessoa { get; set; } = 0L;


        public long WNumContratoTerc { get; set; } = 0L;


        public long WNumContratoAnt { get; set; } = 0L;


        public long WNumContrApolice { get; set; } = 0L;


        public long WNumContrato { get; set; } = 0L;


        public long WNumOcupacao { get; set; } = 0L;


        public long WDdi { get; set; } = 0L;


        public long WTotGravados { get; set; } = 0L;


        public long WTotLidosCertComp { get; set; } = 0L;


        public long WTotLidosCertPact { get; set; } = 0L;


        public long WQtdCompra1 { get; set; } = 0L;


        public long WQtdCompra2 { get; set; } = 0L;


        public long WQtdCancel { get; set; } = 0L;


        public string WsDtRef { get; set; } = Utility.PadLeft("", 10, " ");


        public string WsDtIni { get; set; } = Utility.PadLeft("", 10, " ");


        public string WsDtFim { get; set; } = Utility.PadLeft("", 10, " ");


        public long ESqlcode { get; set; } = 0L;


        public long ESmallint1 { get; set; } = 0L;


        public long EInteger1 { get; set; } = 0L;


        public long EInteger2 { get; set; } = 0L;


        public long EInteger3 { get; set; } = 0L;


        public long EInteger4 { get; set; } = 0L;


        public long EBigint1 { get; set; } = 0L;


        public string HDtaCurrent { get; set; } = Utility.PadLeft("", 10, "\0");


        public long HNumVersaoSerie { get; set; } = 0L;


        public long HCodProdutoMin { get; set; } = 0L;


        public long HCodProdutoMax { get; set; } = 0L;


        public long HCodAcopladoMin { get; set; } = 0L;


        public long HCodAcopladoMax { get; set; } = 0L;


        public string HDtaIniVigenciaAtual { get; set; } = Utility.PadLeft("", 10, "\0");


        public string HDtaIniVigenciaOrig { get; set; } = Utility.PadLeft("", 10, "\0");


        public string HDtaFimCarenciaOrig { get; set; } = Utility.PadLeft("", 10, "\0");


        public long NSz012NumContratoAnt { get; set; } = 0L;


        public Ftm01HRegHeader Ftm01HRegHeader { get; set; } = new Ftm01HRegHeader();


        public Ftm01DRegDetalhe Ftm01DRegDetalhe { get; set; } = new Ftm01DRegDetalhe();


        public Ftm01TRegTrailler Ftm01TRegTrailler { get; set; } = new Ftm01TRegTrailler();


        public Szemb188Parametros Szemb188Parametros { get; set; } = new Szemb188Parametros();


        public DclszPessoa DclszPessoa { get; set; } = new DclszPessoa();


        public DclszApolice DclszApolice { get; set; } = new DclszApolice();


        public DclszOrigemContrato DclszOrigemContrato { get; set; } = new DclszOrigemContrato();


        public DclszContrTerc DclszContrTerc { get; set; } = new DclszContrTerc();


        public DclszContrSeguro DclszContrSeguro { get; set; } = new DclszContrSeguro();


        public DclszObjAcopladoAssist DclszObjAcopladoAssist { get; set; } = new DclszObjAcopladoAssist();


        public DclszPessoaFisica DclszPessoaFisica { get; set; } = new DclszPessoaFisica();


        public DclszPessoaTelefone DclszPessoaTelefone { get; set; } = new DclszPessoaTelefone();


        public DclszEndereco DclszEndereco { get; set; } = new DclszEndereco();


        public DclszAcoplado DclszAcoplado { get; set; } = new DclszAcoplado();


        public DclszApolAcoplado DclszApolAcoplado { get; set; } = new DclszApolAcoplado();


        public DclszObjEndereco DclszObjEndereco { get; set; } = new DclszObjEndereco();


        public DclszAcopladoAssist DclszAcopladoAssist { get; set; } = new DclszAcopladoAssist();


        public DclszObjAcoplado DclszObjAcoplado { get; set; } = new DclszObjAcoplado();


        public Dclcbo Dclcbo { get; set; } = new Dclcbo();


        public Sqlca Sqlca { get; set; } = new Sqlca();


        public HSzl01Szemnl01 HSzl01Szemnl01 { get; set; } = new HSzl01Szemnl01();


        public NSzl01Szemnl01 NSzl01Szemnl01 { get; set; } = new NSzl01Szemnl01();


        public string WGe3000BProgCall { get; set; } = Utility.PadLeft("", 8, " ");


        public string WGe3000BParagrafoOrig { get; set; } = Utility.PadLeft("", 5, " ");


        public long WGe3000BReturnCode { get; set; } = 0L;


        public string WGe3000BFimArq { get; set; } = "NAO";


        public long WGe3000BCount { get; set; } = 0L;


        public long WGe3000BIndErroIni { get; set; } = 0L;


        public long WGe3000BIndErroGra { get; set; } = 0L;


        public long WGe3000BIndErroFin { get; set; } = 0L;


        public string WGe3000BMsgErroIni { get; set; } = Utility.PadLeft("", 120, " ");


        public string WGe3000BMsgErroGra { get; set; } = Utility.PadLeft("", 120, " ");


        public string WGe3000BMsgErroFin { get; set; } = Utility.PadLeft("", 120, " ");


        public string WGe3000BMsgErro { get; set; } = Utility.PadLeft("", 120, " ");


        // W-GE3000B-CT-TRACE-ON - "SIM"
        // W-GE3000B-CT-TRACE-DET-ON - "SIM"
        public string WGe3000BCtTrace { get; set; } = "NAO";


        public LkGe3000BParametros LkGe3000BParametros { get; set; } = new LkGe3000BParametros();


        public Szemb188FileParametros Szemb188FileParametros { get; set; } = new Szemb188FileParametros();


        public static OrderedDictionary DataTypeMap { get; set; } = new OrderedDictionary();


        public FileOrganization<string> Arqtemp1 { get; set; } = new SequentialFile<string>();


        public long RETURN_CODE { get; set; } = 0;


        public CursorResult<NamedQueryResponseSzemb188Crpactuante> szemb188CursorCrPactuante { get; set; } = default;


        public CursorResult<NamedQueryResponseSzemb188CrCertifCompra> szemb188CursorCrCertifCompra { get; set; } = default;



        public long Execute(Szemb188FileParametros Szemb188FileParametros)
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Execute");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                this.Szemb188FileParametros = Szemb188FileParametros;

                Szemb188P0000principal();

            }
            catch (Exception e) when (e is GoBackException || e is ExitException)
            {
                LogUtils.WriteLine(e.Message + " Flow is returning from Szemb188");
            }

            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Execute Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
            return RETURN_CODE;
        }

        private void Szemb188P0000principal()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188P0000principal");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE 'P0000'                  TO W-LABEL */
            WLabel = Utility.PadStrToSize("P0000", 5);


            /* DISPLAY ' ' */
            Console.WriteLine(" ");


            /* DISPLAY 'SZEMB188 - VERSAO 002 - INICIOU PROCESSAMENTO EM: '
                          FUNCTION CURRENT-DATE(07:2) '/'
                          FUNCTION CURRENT-DATE(05:2) '/'
                          FUNCTION CURRENT-DATE(01:4) ' AS '
                          FUNCTION CURRENT-DATE(09:2) ':'
                          FUNCTION CURRENT-DATE(11:2) ':'
                          FUNCTION CURRENT-DATE(13:2) ' ***' */
            Console.WriteLine("SZEMB188 - VERSAO 002 - INICIOU PROCESSAMENTO EM: " + Utility.GetCurrentDate().SafeSubstring((int)07 - 1, 2) + "/" + Utility.GetCurrentDate().SafeSubstring((int)05 - 1, 2) + "/" + Utility.GetCurrentDate().SafeSubstring((int)01 - 1, 4) + " AS " + Utility.GetCurrentDate().SafeSubstring((int)09 - 1, 2) + ":" + Utility.GetCurrentDate().SafeSubstring((int)11 - 1, 2) + ":" + Utility.GetCurrentDate().SafeSubstring((int)13 - 1, 2) + " ***");


            /* DISPLAY ' ' */
            Console.WriteLine(" ");


            /* PERFORM P1000-INICIALIZA */
            Szemb188P1000inicializa();


            /* PERFORM P2000-PRINCIPAL-COMPRA */
            Szemb188P2000principalcompra();


            /* MOVE  00                      TO  W-RETURN-CODE */
            WReturnCode = Math.Abs(Convert.ToInt64(0));


            /* PERFORM P9000-FINALIZA */
            Szemb188P9000finaliza();


            /* GO TO P9999-FINALIZACAO */
            Szemb188P9999finalizacao();


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188P0000principal Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188P1000inicializa()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188P1000inicializa");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE 'P1000'                  TO W-LABEL */
            WLabel = Utility.PadStrToSize("P1000", 5);


            /* INITIALIZE W-PROGRAMA-VERSAO
                                  W-RETURN-CODE */
            WProgramaVersao = Utility.PadLeft("", 80, " ");
            WReturnCode = 0;


            /* STRING W-PROGRAMA '-V.08-393786-ARQUIVO-TEMPO ASSISTENCIA-'
                              FUNCTION WHEN-COMPILED(1:12)
                            DELIMITED BY SIZE INTO W-PROGRAMA-VERSAO */
            StringBuilder builder1 = new StringBuilder();
            builder1.Append(WPrograma).Append("-V.08-393786-ARQUIVO-TEMPO ASSISTENCIA-").Append(Utility.WhenCompiled().SafeSubstring((int)1 - 1, 12));
            string builderOut1 = builder1.ToString();
            if (builderOut1.Length > 80)
            {
                builderOut1 = builderOut1.SafeSubstring(0, 80);
            }
            WProgramaVersao = builderOut1;


            /* DISPLAY 'PROCESSAMENTO PARA GERAR ARQUIVO PARA A TEMPO.' */
            Console.WriteLine("PROCESSAMENTO PARA GERAR ARQUIVO PARA A TEMPO.");


            /* INITIALIZE W-SEQ-REGISTRO */
            WSeqRegistro = 0;


            /* PERFORM P1200-INICIALIZAR-MONITORACAO */
            Szemb188P1200inicializarmonitoracao();


            /* OPEN OUTPUT ARQTEMP1 */
            Arqtemp1.Open(Path.Combine(GetWorkArea(), SerializerUtils.Serialize(Szemb188FileParametros.WsParNomArqArqtemp1).Trim()), Mode.OUTPUT);


            /* MOVE 'SIM'                    TO W-OPEN-ARQ */
            WOpenArq = Utility.PadStrToSize("SIM", 3);


            /* MOVE +2147483647              TO W-INTEGER-VALOR-MAX */
            WIntegerValorMax = Convert.ToInt64(+2147483647);


            /* MOVE 0                        TO H-COD-PRODUTO-MIN */
            HCodProdutoMin = Convert.ToInt64(0);


            /* MOVE W-INTEGER-VALOR-MAX      TO H-COD-PRODUTO-MAX */
            HCodProdutoMax = Convert.ToInt64(WIntegerValorMax);


            /* MOVE 0                        TO H-COD-ACOPLADO-MIN */
            HCodAcopladoMin = Convert.ToInt64(0);


            /* MOVE W-INTEGER-VALOR-MAX      TO H-COD-ACOPLADO-MAX */
            HCodAcopladoMax = Convert.ToInt64(WIntegerValorMax);


            /* *>EXECSQL EXEC SQL
                  *>EXECSQL SELECT CURRENT DATE   - 1 MONTH
                  *>EXECSQL INTO :WS-DT-REF
                  *>EXECSQL
                  *>EXECSQL END-EXEC }

                  *>EXECSQL EXEC SQL
                  *>EXECSQL SELECT DATE(SUBSTR(CHAR(DATE(:WS-DT-REF)),1,7)
                  *>EXECSQL ||'-01')
                  *>EXECSQL , DATE(SUBSTR(CHAR(DATE(:WS-DT-REF)
                  *>EXECSQL + 1 MONTH),1,7)||'-01') - 1 DAY
                  *>EXECSQL INTO :WS-DT-INI
                  *>EXECSQL ,:WS-DT-FIM
                  *>EXECSQL
                  *>EXECSQL END-EXEC } */
            // Create the input Object
            NamedQueryInputSzemb188P1000Inicializa NamedQueryInputSzemb188P1000InicializaObj = new NamedQueryInputSzemb188P1000Inicializa();

            // Execute the NamedQuery
            NHibernateUtilSzemb188 nhUtil = new NHibernateUtilSzemb188(Sqlca);
            List<NamedQueryResponseSzemb188P1000Inicializa> NamedQueryResponseSzemb188P1000InicializaResp = nhUtil.ExecuteSelectNamedQuerySzemb188P1000Inicializa(NamedQueryInputSzemb188P1000InicializaObj);
            if (NamedQueryResponseSzemb188P1000InicializaResp.Count > 0)
            {
                // Extract data from the output object
                for (int i = 0; i < NamedQueryResponseSzemb188P1000InicializaResp.Count; i++)
                {
                    NamedQueryResponseSzemb188P1000Inicializa obj = NamedQueryResponseSzemb188P1000InicializaResp[i];
                    WsDtRef = obj.Currentdate1Month;
                }
            }
            // Create the input Object
            NamedQueryInputSzemb188P1000InicializaQuery1 NamedQueryInputSzemb188P1000InicializaQuery1Obj = new NamedQueryInputSzemb188P1000InicializaQuery1();
            NamedQueryInputSzemb188P1000InicializaQuery1Obj.WsDtRef = WsDtRef.ToString();

            // Execute the NamedQuery
            NHibernateUtilSzemb188 nhUtil_query1 = new NHibernateUtilSzemb188(Sqlca);
            List<NamedQueryResponseSzemb188P1000InicializaQuery1> NamedQueryResponseSzemb188P1000InicializaQuery1Resp = nhUtil_query1.ExecuteSelectNamedQuerySzemb188P1000InicializaQuery1(NamedQueryInputSzemb188P1000InicializaQuery1Obj);
            if (NamedQueryResponseSzemb188P1000InicializaQuery1Resp.Count > 0)
            {
                // Extract data from the output object
                for (int i = 0; i < NamedQueryResponseSzemb188P1000InicializaQuery1Resp.Count; i++)
                {
                    NamedQueryResponseSzemb188P1000InicializaQuery1 obj = NamedQueryResponseSzemb188P1000InicializaQuery1Resp[i];
                    WsDtIni = obj.DateSubstrCharDateW9E668A;
                    WsDtFim = obj.DateSubstrCharDateW21B5Df;
                }
            }


            /* DISPLAY 'WS-DT-INI = ' WS-DT-INI */
            Console.WriteLine("WS-DT-INI = " + WsDtIni);


            /* DISPLAY 'WS-DT-FIM = ' WS-DT-FIM */
            Console.WriteLine("WS-DT-FIM = " + WsDtFim);


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188P1000inicializa Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188P1200inicializarmonitoracao()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188P1200inicializarmonitoracao");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE 'P1200'                  TO W-LABEL */
            WLabel = Utility.PadStrToSize("P1200", 5);


            /* INITIALIZE LK-GE3000B-PARAMETROS
                                  H-SZL01-SEQ-LOG-SISTEMA */
            LkGe3000BParametros.Initialize();
            HSzl01Szemnl01.HSzl01SeqLogSistema = 0;


            /* MOVE W-PROGRAMA               TO LK-GE3000B-COD-PROGRAMA
                                                        LK-GE3000B-COD-USUARIO */
            LkGe3000BParametros.LkGe3000BCodPrograma = Utility.PadStrToSize(WPrograma, 10);
            LkGe3000BParametros.LkGe3000BCodUsuario = Utility.PadStrToSize(WPrograma, 8);


            /* IF SZEMB188-TRACE-ON-88
                          MOVE 'SIM'                 TO LK-GE3000B-TRACE
                       END-IF */
            if (Szemb188Parametros.Szemb188Szemb188traceon88())
            {
                LkGe3000BParametros.LkGe3000BTrace = Utility.PadStrToSize("SIM", 3);


            }


            /* IF SZEMB188-TRACE-ON-88
                          DISPLAY W-PROGRAMA '-VAI CALL PMONITOR: PARAMETROS='
                                  ' OPERACAO<' LK-GE3000B-OPERACAO '>'
                                  ' PROC-ARQ<' LK-GE3000B-PROC-ARQ '>'
                          DISPLAY 'P1<' LK-GE3000B-PARAMETROS(001:100) '>'
                          DISPLAY 'P2<' LK-GE3000B-PARAMETROS(101:100) '>'
                       END-IF */
            if (Szemb188Parametros.Szemb188Szemb188traceon88())
            {
                Console.WriteLine(WPrograma + "-VAI CALL PMONITOR: PARAMETROS=" + " OPERACAO<" + LkGe3000BParametros.LkGe3000BOperacao + ">" + " PROC-ARQ<" + LkGe3000BParametros.LkGe3000BProcArq + ">");


                Console.WriteLine("P1<" + Utility.ToJsonString(Utility.RefMod(LkGe3000BParametros, (DataTypeMetadata)DataTypeMap["LkGe3000BParametros"], 0, 100)) + ">");


                Console.WriteLine("P2<" + Utility.ToJsonString(Utility.RefMod(LkGe3000BParametros, (DataTypeMetadata)DataTypeMap["LkGe3000BParametros"], 100, 100)) + ">");


            }


            /* PERFORM PMONITOR-INICIALIZACAO */
            Szemb188Pmonitorinicializacao();


            /* MOVE 'TEM'                    TO LK-GE3000B-COD-TP-ARQUIVO */
            LkGe3000BParametros.LkGe3000BCodTpArquivo = Utility.PadStrToSize("TEM", 3);


            /* MOVE 'ARQTEMP1'               TO LK-GE3000B-ASSIGN-DDNAME */
            LkGe3000BParametros.LkGe3000BAssignDdname = Utility.PadStrToSize("ARQTEMP1", 8);


            /* MOVE 'FTM01'                  TO LK-GE3000B-COD-LEIAUTE */
            LkGe3000BParametros.LkGe3000BCodLeiaute = Utility.PadStrToSize("FTM01", 10);


            /* PERFORM PMONITOR-GRAVA-MONITOR */
            Szemb188Pmonitorgravamonitor();


            /* PERFORM DB900-EXECUTA-COMMIT */
            Szemb188Db900executacommit();


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188P1200inicializarmonitoracao Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188P2000principalcompra()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188P2000principalcompra");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE 'P2000'                  TO W-LABEL */
            WLabel = Utility.PadStrToSize("P2000", 5);


            /* MOVE SPACES                   TO W-FIM-CERTIF */
            WFimCertif = Utility.PadStrToSize(" ", 3);


            /* MOVE H-COD-PRODUTO-MIN        TO E-INTEGER-1 */
            EInteger1 = Math.Abs(Convert.ToInt64(HCodProdutoMin));


            /* MOVE H-COD-PRODUTO-MAX        TO E-INTEGER-2 */
            EInteger2 = Math.Abs(Convert.ToInt64(HCodProdutoMax));


            /* MOVE H-COD-ACOPLADO-MIN       TO E-INTEGER-3 */
            EInteger3 = Math.Abs(Convert.ToInt64(HCodAcopladoMin));


            /* MOVE H-COD-ACOPLADO-MAX       TO E-INTEGER-4 */
            EInteger4 = Math.Abs(Convert.ToInt64(HCodAcopladoMax));


            /* PERFORM DB010-OPEN-CR-CERT-COMPRA */
            Szemb188Db010opencrcertcompra();


            /* PERFORM DB020-FETCH-CERT-COMPRA */
            Szemb188Db020fetchcertcompra();


            /* DISPLAY ' ' */
            Console.WriteLine(" ");


            /* DISPLAY W-PROGRAMA '-FEZ PRIMEIRO FETCH CURSOR '
                               'CR_CERTIF_COMPRA AS '  FUNCTION CURRENT-DATE */
            Console.WriteLine(WPrograma + "-FEZ PRIMEIRO FETCH CURSOR " + "CR_CERTIF_COMPRA AS " + Utility.GetCurrentDate());


            /* PERFORM UNTIL W-FIM-CERTIF = 'SIM'
                  *>       --- ACESSA PACTUANTE
                         PERFORM DB030-ABRIR-PACTUANTE
                         PERFORM DB040-LER-PACTUANTE

                         IF W-FIM-PACTUANTE = 'SIM'
                  *>          --- GRAVA LOG
                            ADD 1                    TO W-QTD-DISPLAY-LOG
                            IF W-QTD-DISPLAY-LOG < 100
                               MOVE SZ012-NUM-CONTRATO-TERC
                                                     TO E-BIGINT-1
                               INITIALIZE W-MENSAGEM-ERRO
                               STRING 'PACTUANTE <' E-BIGINT-1 '> NAO ENCONTRADO '
                                 DELIMITED BY SIZE INTO W-MENSAGEM-ERRO(001:80)
                               STRING 'PARA O CONTRATO <' W-NUM-CONTR-APOLICE '>'
                                 DELIMITED BY SIZE INTO W-MENSAGEM-ERRO(081:80)
                              MOVE 0                TO H-SZL01-IND-ERRO-LOG
                              MOVE 0                TO N-SZL01-IND-ERRO-LOG
                              PERFORM C0010-CALL-SP-SZEMNL01
                           END-IF
                        ELSE
                  *>         --- ACESSA ENDERECO
                            PERFORM DB050-LER-ENDERECO

                  *>         --- ACESSA INICIO DE VIGENCIA
                            IF N-SZ012-NUM-CONTRATO-ANT = 0
                            AND SZ012-NUM-CONTRATO-ANT NOT = SZ012-NUM-CONTRATO-TERC
                               PERFORM DB060-ACESSA-VIGENCIA-CONTR
                            END-IF
                         END-IF

                  *>      --- PROCESSA PACTUANTES

                         PERFORM UNTIL W-FIM-PACTUANTE = 'SIM'
                  *>        --- GERA CABECALHO
                           IF W-TOT-GRAVADOS = 0
                              PERFORM P2800-GRAVA-CABECALHO
                          END-IF
                  *>        --- MONTA DETALHE
                           PERFORM P2100-TRATAR-SAF
                  *>        --- GRAVA DETALHE
                           PERFORM P2810-GRAVA-DETALHE
                  *>         --- ATUALIZA ACOPLADO
                  *>         --- ACESSA PROXIMO PACTUANTE
                           PERFORM DB040-LER-PACTUANTE
                         END-PERFORM
                  *>       --- PROXIMO CERTIFICADO
                         PERFORM DB020-FETCH-CERT-COMPRA
                       END-PERFORM */
            while (!((Comparisions.SafeStringCompare(WFimCertif, "SIM"))))
            {
                Szemb188Db030abrirpactuante();
                Szemb188Db040lerpactuante();
                if ((Comparisions.SafeStringCompare(WFimPactuante, "SIM")))
                {
                    WQtdDisplayLog = Math.Abs(Utility.Truncate(WQtdDisplayLog + 1));


                    if (WQtdDisplayLog < 100)
                    {
                        EBigint1 = Math.Abs(Convert.ToInt64(DclszContrTerc.Sz012NumContratoTerc));


                        WMensagemErro = Utility.PadLeft("", 500, " ");


                        StringBuilder builder1 = new StringBuilder();
                        builder1.Append("PACTUANTE <").Append(SerializerUtils.Serialize(EBigint1, (DataTypeMetadata)DataTypeMap["EBigint1"])).Append("> NAO ENCONTRADO ");
                        WMensagemErro = Utility.CopyString(builder1.ToString(), WMensagemErro, 0, 0, 79);


                        StringBuilder builder2 = new StringBuilder();
                        builder2.Append("PARA O CONTRATO <").Append(SerializerUtils.Serialize(WNumContrApolice, (DataTypeMetadata)DataTypeMap["WNumContrApolice"])).Append(">");
                        WMensagemErro = Utility.CopyString(builder2.ToString(), WMensagemErro, 0, 80, 79);


                        HSzl01Szemnl01.HSzl01IndErroLog = Convert.ToInt64(0);


                        NSzl01Szemnl01.NSzl01IndErroLog = Convert.ToInt64(0);


                        Szemb188C0010callspszemnl01();


                    }


                }
                else
                {
                    Szemb188Db050lerendereco();



                    if (NSz012NumContratoAnt == 0 && DclszContrTerc.Sz012NumContratoAnt != DclszContrTerc.Sz012NumContratoTerc)
                    {
                        Szemb188Db060acessavigenciacontr();


                    }



                }
                while (!((Comparisions.SafeStringCompare(WFimPactuante, "SIM"))))
                {
                    if (WTotGravados == 0)
                    {
                        Szemb188P2800gravacabecalho();


                    }
                    Szemb188P2100tratarsaf();
                    Szemb188P2810gravadetalhe();
                    Szemb188Db040lerpactuante();

                }
                Szemb188Db020fetchcertcompra();

            }


            /* IF W-TOT-GRAVADOS > 0
                          PERFORM P2820-GRAVA-TRAILLER
                       END-IF */
            if (WTotGravados > 0)
            {
                Szemb188P2820gravatrailler();


            }


            /* IF W-TOT-LIDOS-CERT-COMP = ZEROS
                  *>        --- GRAVA LOG
                          INITIALIZE W-MENSAGEM-ERRO
                          STRING 'NAO FOI ENCONTRADO CERTIFICADO PARA O PRODUTO '
                                 SZEMB188-COD-PRODUTO-X '. '
                                 DELIMITED BY SIZE INTO W-MENSAGEM-ERRO(001:80)
                          STRING 'PRODUTO E/OU CONTRATO SEM DIREITO A CERTIFICADO.'
                                 DELIMITED BY SIZE INTO W-MENSAGEM-ERRO(081:80)
                          MOVE 0                     TO H-SZL01-IND-ERRO-LOG
                          MOVE 0                     TO N-SZL01-IND-ERRO-LOG
                          PERFORM C0010-CALL-SP-SZEMNL01
                       END-IF */
            if (WTotLidosCertComp == 0)
            {
                WMensagemErro = Utility.PadLeft("", 500, " ");


                StringBuilder builder3 = new StringBuilder();
                builder3.Append("NAO FOI ENCONTRADO CERTIFICADO PARA O PRODUTO ").Append(SerializerUtils.Serialize(Szemb188Parametros.Szemb188CodProdutoX, (DataTypeMetadata)Szemb188CodProdutoX.DataTypeMap["Szemb188CodProdutoX"])).Append(". ");
                WMensagemErro = Utility.CopyString(builder3.ToString(), WMensagemErro, 0, 0, 79);


                StringBuilder builder4 = new StringBuilder();
                builder4.Append("PRODUTO E/OU CONTRATO SEM DIREITO A CERTIFICADO.");
                WMensagemErro = Utility.CopyString(builder4.ToString(), WMensagemErro, 0, 80, 79);


                HSzl01Szemnl01.HSzl01IndErroLog = Convert.ToInt64(0);


                NSzl01Szemnl01.NSzl01IndErroLog = Convert.ToInt64(0);


                Szemb188C0010callspszemnl01();


            }


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188P2000principalcompra Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188P2100tratarsaf()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188P2100tratarsaf");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE 'P2100'                  TO W-LABEL */
            WLabel = Utility.PadStrToSize("P2100", 5);


            /* STRING
                            SZ115-DTA-INI-VIGENCIA(1:4)
                            SZ115-DTA-INI-VIGENCIA(6:2)
                            SZ115-DTA-INI-VIGENCIA(9:2)
                            DELIMITED BY SIZE      INTO FTM01-D-DTA-INICIO
                       END-STRING */
            StringBuilder builder1 = new StringBuilder();
            builder1.Append(Utility.RefMod(DclszObjAcoplado.Sz115DtaIniVigencia, (DataTypeMetadata)DclszObjAcoplado.DataTypeMap["Sz115DtaIniVigencia"], 0, 4)).Append(Utility.RefMod(DclszObjAcoplado.Sz115DtaIniVigencia, (DataTypeMetadata)DclszObjAcoplado.DataTypeMap["Sz115DtaIniVigencia"], 5, 2)).Append(Utility.RefMod(DclszObjAcoplado.Sz115DtaIniVigencia, (DataTypeMetadata)DclszObjAcoplado.DataTypeMap["Sz115DtaIniVigencia"], 8, 2));
            Ftm01DRegDetalhe.Ftm01DDtaInicio = Convert.ToInt64(builder1.ToString());


            /* STRING
                            SZ115-DTA-FIM-VIGENCIA(1:4)
                            SZ115-DTA-FIM-VIGENCIA(6:2)
                            SZ115-DTA-FIM-VIGENCIA(9:2)
                            DELIMITED BY SIZE      INTO FTM01-D-DTA-FIM
                       END-STRING */
            StringBuilder builder2 = new StringBuilder();
            builder2.Append(Utility.RefMod(DclszObjAcoplado.Sz115DtaFimVigencia, (DataTypeMetadata)DclszObjAcoplado.DataTypeMap["Sz115DtaFimVigencia"], 0, 4)).Append(Utility.RefMod(DclszObjAcoplado.Sz115DtaFimVigencia, (DataTypeMetadata)DclszObjAcoplado.DataTypeMap["Sz115DtaFimVigencia"], 5, 2)).Append(Utility.RefMod(DclszObjAcoplado.Sz115DtaFimVigencia, (DataTypeMetadata)DclszObjAcoplado.DataTypeMap["Sz115DtaFimVigencia"], 8, 2));
            Ftm01DRegDetalhe.Ftm01DDtaFim = Convert.ToInt64(builder2.ToString());


            /* MOVE SPACES                   TO FTM01-D-NUM-ITEM */
            Ftm01DRegDetalhe.Ftm01DNumItem = Utility.PadStrToSize(" ", 5);


            /* MOVE SZ010-NUM-APOLICE        TO FTM01-D-NUM-CARTAO */
            Ftm01DRegDetalhe.Ftm01DNumCartao = Utility.PadRight(Utility.PadLeft(DclszApolice.Sz010NumApolice.ToString(), 18, '0'), 30, ' ');


            /* MOVE SZ011-COD-PRODUTO        TO FTM01-D-COD-SEGURO */
            Ftm01DRegDetalhe.Ftm01DCodSeguro = Math.Abs(Convert.ToInt64(DclszOrigemContrato.Sz011CodProduto));


            /* MOVE SZ043-IND-ENVIO          TO FTM01-D-SIT-REGISTRO */
            Ftm01DRegDetalhe.Ftm01DSitRegistro = Utility.PadStrToSize(DclszObjAcopladoAssist.Sz043IndEnvio, 1);


            /* MOVE W-NUM-CONTR-APOLICE      TO FTM01-D-NUM-APOLICE */
            Ftm01DRegDetalhe.Ftm01DNumApolice = Utility.PadRight(Utility.PadLeft(WNumContrApolice.ToString(), 18, '0'), 40, ' ');


            /* MOVE SZ063-NOM-LOGRADOURO     TO FTM01-D-END-SEGURADO */
            Ftm01DRegDetalhe.Ftm01DEndSegurado = Utility.PadStrToSize(DclszEndereco.Sz063NomLogradouro, 60);


            /* MOVE SZ063-NUM-LOGRADOURO     TO FTM01-D-NUM-ENDERECO */
            Ftm01DRegDetalhe.Ftm01DNumEndereco = Utility.PadStrToSize(DclszEndereco.Sz063NumLogradouro, 6);


            /* MOVE SZ063-DES-COMPL-ENDERECO TO FTM01-D-END-COMPLEME */
            Ftm01DRegDetalhe.Ftm01DEndCompleme = Utility.PadStrToSize(DclszEndereco.Sz063DesComplEndereco, 40);


            /* MOVE SZ063-NOM-CIDADE         TO FTM01-D-NOM-CIDADE */
            Ftm01DRegDetalhe.Ftm01DNomCidade = Utility.PadStrToSize(DclszEndereco.Sz063NomCidade, 62);


            /* MOVE SZ063-NOM-BAIRRO         TO FTM01-D-NOM-BAIRRO */
            Ftm01DRegDetalhe.Ftm01DNomBairro = Utility.PadStrToSize(DclszEndereco.Sz063NomBairro, 40);


            /* COMPUTE FTM01-D-CEP-CIDADE = FUNCTION NUMVAL-C
                                                     (SZ063-COD-CEP) */
            Ftm01DRegDetalhe.Ftm01DCepCidade = Math.Abs(Utility.Truncate(Utility.NumericValueC(DclszEndereco.Sz063CodCep)));


            /* MOVE 'BRASIL'                 TO FTM01-D-NOM-PAIS */
            Ftm01DRegDetalhe.Ftm01DNomPais = Utility.PadStrToSize("BRASIL", 20);


            /* PERFORM DB070-LER-TELEFONE */
            Szemb188Db070lertelefone();


            /* MOVE W-DDI                    TO FTM01-D-DDI-TEL */
            Ftm01DRegDetalhe.Ftm01DDdiTel = Math.Abs(Convert.ToInt64(Utility.PadNumToSize(WDdi, 3)));


            /* MOVE SZ057-NUM-DDD            TO FTM01-D-DDD-TEL */
            Ftm01DRegDetalhe.Ftm01DDddTel = Math.Abs(Convert.ToInt64(Utility.PadNumToSize(DclszPessoaTelefone.Sz057NumDdd, 3)));


            /* MOVE SZ057-NUM-TELEFONE       TO FTM01-D-NUM-TEL */
            Ftm01DRegDetalhe.Ftm01DNumTel = Math.Abs(Convert.ToInt64(DclszPessoaTelefone.Sz057NumTelefone));


            /* MOVE SZ008-NOM-RAZ-SOCIAL     TO FTM01-D-NOM-SEGURADO */
            Ftm01DRegDetalhe.Ftm01DNomSegurado = Utility.PadStrToSize(DclszPessoa.Sz008NomRazSocial, 40);


            /* MOVE SZ008-NUM-CPF-CNPJ       TO FTM01-D-CPF-CNPJ */
            Ftm01DRegDetalhe.Ftm01DCpfCnpj = Math.Abs(Convert.ToInt64(Utility.PadNumToSize(DclszPessoa.Sz008NumCpfCnpj, 14)));


            /* PERFORM DB080-DESC-PROFISSAO */
            Szemb188Db080descprofissao();


            /* MOVE CBO-DESCR-CBO            TO FTM01-D-NOM-PROFISSAO */
            Ftm01DRegDetalhe.Ftm01DNomProfissao = Utility.PadStrToSize(Dclcbo.CboDescrCbo, 60);


            /* IF SZ053-IND-SEXO = 'M'
                          MOVE 1                     TO FTM01-D-COD-SEXO
                       ELSE
                          MOVE 2                     TO FTM01-D-COD-SEXO
                       END-IF */
            if ((Comparisions.SafeStringCompare(DclszPessoaFisica.Sz053IndSexo, "M")))
            {
                Ftm01DRegDetalhe.Ftm01DCodSexo = Math.Abs(Convert.ToInt64(1));


            }
            else
            {
                Ftm01DRegDetalhe.Ftm01DCodSexo = Math.Abs(Convert.ToInt64(2));



            }


            /* INITIALIZE FTM01-D-DTA-NASC */
            Ftm01DRegDetalhe.Ftm01DDtaNasc = 0;


            /* STRING SZ053-DTA-NASCIMENTO(1:4)
                              SZ053-DTA-NASCIMENTO(6:2)
                              SZ053-DTA-NASCIMENTO(9:2)
                            DELIMITED BY SIZE INTO FTM01-D-DTA-NASC
                       END-STRING */
            StringBuilder builder3 = new StringBuilder();
            builder3.Append(Utility.RefMod(DclszPessoaFisica.Sz053DtaNascimento, (DataTypeMetadata)DclszPessoaFisica.DataTypeMap["Sz053DtaNascimento"], 0, 4)).Append(Utility.RefMod(DclszPessoaFisica.Sz053DtaNascimento, (DataTypeMetadata)DclszPessoaFisica.DataTypeMap["Sz053DtaNascimento"], 5, 2)).Append(Utility.RefMod(DclszPessoaFisica.Sz053DtaNascimento, (DataTypeMetadata)DclszPessoaFisica.DataTypeMap["Sz053DtaNascimento"], 8, 2));
            Ftm01DRegDetalhe.Ftm01DDtaNasc = Convert.ToInt64(builder3.ToString());


            /* MOVE ALL '0'                  TO FTM01-D-COD-RG */
            Ftm01DRegDetalhe.Ftm01DCodRg = Math.Abs(Convert.ToInt64("0"));


            /* MOVE SZ053-IND-ESTADO-CIVIL   TO FTM01-D-EST-CIVIL */
            Ftm01DRegDetalhe.Ftm01DEstCivil = Utility.ParseLongSafe(DclszPessoaFisica.Sz053IndEstadoCivil);


            /* INITIALIZE FTM01-D-DTA-INI-VIG-SEG */
            Ftm01DRegDetalhe.Ftm01DDtaIniVigSeg = 0;


            /* STRING SZ021-DTA-INI-VIG-SEG(1:4)
                              SZ021-DTA-INI-VIG-SEG(6:2)
                              SZ021-DTA-INI-VIG-SEG(9:2)
                            DELIMITED BY SIZE INTO FTM01-D-DTA-INI-VIG-SEG */
            StringBuilder builder4 = new StringBuilder();
            builder4.Append(Utility.RefMod(DclszContrSeguro.Sz021DtaIniVigSeg, (DataTypeMetadata)DclszContrSeguro.DataTypeMap["Sz021DtaIniVigSeg"], 0, 4)).Append(Utility.RefMod(DclszContrSeguro.Sz021DtaIniVigSeg, (DataTypeMetadata)DclszContrSeguro.DataTypeMap["Sz021DtaIniVigSeg"], 5, 2)).Append(Utility.RefMod(DclszContrSeguro.Sz021DtaIniVigSeg, (DataTypeMetadata)DclszContrSeguro.DataTypeMap["Sz021DtaIniVigSeg"], 8, 2));
            Ftm01DRegDetalhe.Ftm01DDtaIniVigSeg = Convert.ToInt64(builder4.ToString());


            /* INITIALIZE FTM01-D-DTA-FIM-VIG-SEG */
            Ftm01DRegDetalhe.Ftm01DDtaFimVigSeg = 0;


            /* STRING SZ021-DTA-FIM-VIG-SEG(1:4)
                              SZ021-DTA-FIM-VIG-SEG(6:2)
                              SZ021-DTA-FIM-VIG-SEG(9:2)
                            DELIMITED BY SIZE INTO FTM01-D-DTA-FIM-VIG-SEG */
            StringBuilder builder5 = new StringBuilder();
            builder5.Append(Utility.RefMod(DclszContrSeguro.Sz021DtaFimVigSeg, (DataTypeMetadata)DclszContrSeguro.DataTypeMap["Sz021DtaFimVigSeg"], 0, 4)).Append(Utility.RefMod(DclszContrSeguro.Sz021DtaFimVigSeg, (DataTypeMetadata)DclszContrSeguro.DataTypeMap["Sz021DtaFimVigSeg"], 5, 2)).Append(Utility.RefMod(DclszContrSeguro.Sz021DtaFimVigSeg, (DataTypeMetadata)DclszContrSeguro.DataTypeMap["Sz021DtaFimVigSeg"], 8, 2));
            Ftm01DRegDetalhe.Ftm01DDtaFimVigSeg = Convert.ToInt64(builder5.ToString());


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188P2100tratarsaf Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188P2800gravacabecalho()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188P2800gravacabecalho");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE 'P2800'                  TO W-LABEL */
            WLabel = Utility.PadStrToSize("P2800", 5);


            /* MOVE 9                        TO FTM01-H-COD-CLIENTE */
            Ftm01HRegHeader.Ftm01HCodCliente = Math.Abs(Convert.ToInt64(602));


            /* MOVE SZ098-COD-PROD-ACOPLADO  TO FTM01-H-COD-PRODUTO */
            Ftm01HRegHeader.Ftm01HCodProduto = Math.Abs(Convert.ToInt64(Utility.PadNumToSize(DclszAcopladoAssist.Sz098CodProdAcoplado, 2)));


            /* MOVE FUNCTION CURRENT-DATE(1:8)
                                                     TO FTM01-H-DTA-GERACAO */
            Ftm01HRegHeader.Ftm01HDtaGeracao = Utility.PadNumToSize(Utility.GetCurrentDate().SafeSubstring((int)1 - 1, 8), 8);


            /* MOVE SZ098-SEQ-ENVIO          TO FTM01-H-NUM-SEQ */
            Ftm01HRegHeader.Ftm01HNumSeq = Math.Abs(Convert.ToInt64(Utility.PadNumToSize(DclszAcopladoAssist.Sz098SeqEnvio, 5)));


            /* WRITE REG-ARQTEMP1 FROM FTM01-H-REG-HEADER */
            Arqtemp1.WriteObject(SerializerUtils.Serialize(Ftm01HRegHeader));


            /* ADD 1                         TO W-SEQ-REGISTRO */
            WSeqRegistro = Math.Abs(Utility.Truncate(WSeqRegistro + 1));


            /* MOVE W-SEQ-REGISTRO           TO LK-GE3000B-NUM-ITEM-MOV */
            LkGe3000BParametros.LkGe3000BNumItemMov = Convert.ToInt64(WSeqRegistro);


            /* INITIALIZE LK-GE3000B-NUM-PES-OPERADOR
                                  LK-GE3000B-NUM-LINHA-PRODUTO
                                  LK-GE3000B-NUM-CONTRATO-TERC
                                  LK-GE3000B-NUM-CONTRATO */
            LkGe3000BParametros.LkGe3000BNumPesOperador = 0;
            LkGe3000BParametros.LkGe3000BNumLinhaProduto = 0;
            LkGe3000BParametros.LkGe3000BNumContratoTerc = 0;
            LkGe3000BParametros.LkGe3000BNumContrato = 0;


            /* MOVE 'H'                      TO LK-GE3000B-COD-TP-REGISTRO */
            LkGe3000BParametros.LkGe3000BCodTpRegistro = Utility.PadStrToSize("H", 2);


            /* MOVE FTM01-H-REG-HEADER       TO LK-GE3000B-TXT-CONTD */
            LkGe3000BParametros.LkGe3000BTxtContd = Utility.PadStrToSize(SerializerUtils.Serialize(Ftm01HRegHeader), 2000);


            /* PERFORM PMONITOR-GRAVA-ARQUIVOS */
            Szemb188Pmonitorgravaarquivos();


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188P2800gravacabecalho Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188P2810gravadetalhe()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188P2810gravadetalhe");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE 'P2810'                  TO W-LABEL */
            WLabel = Utility.PadStrToSize("P2810", 5);


            /* ADD 1                         TO W-TOT-GRAVADOS */
            WTotGravados = Math.Abs(Utility.Truncate(WTotGravados + 1));


            /* WRITE REG-ARQTEMP1   FROM FTM01-D-REG-DETALHE */
            Arqtemp1.WriteObject(SerializerUtils.Serialize(Ftm01DRegDetalhe));


            /* ADD 1                         TO W-SEQ-REGISTRO */
            WSeqRegistro = Math.Abs(Utility.Truncate(WSeqRegistro + 1));


            /* MOVE W-SEQ-REGISTRO           TO LK-GE3000B-NUM-ITEM-MOV */
            LkGe3000BParametros.LkGe3000BNumItemMov = Convert.ToInt64(WSeqRegistro);


            /* MOVE SZ012-NUM-PES-OPERADOR   TO LK-GE3000B-NUM-PES-OPERADOR */
            LkGe3000BParametros.LkGe3000BNumPesOperador = Convert.ToInt64(DclszContrTerc.Sz012NumPesOperador);


            /* MOVE SZ012-NUM-LINHA-PRODUTO  TO LK-GE3000B-NUM-LINHA-PRODUTO */
            LkGe3000BParametros.LkGe3000BNumLinhaProduto = Convert.ToInt64(DclszContrTerc.Sz012NumLinhaProduto);


            /* MOVE SZ012-NUM-CONTRATO-TERC  TO LK-GE3000B-NUM-CONTRATO-TERC */
            LkGe3000BParametros.LkGe3000BNumContratoTerc = Convert.ToInt64(DclszContrTerc.Sz012NumContratoTerc);


            /* MOVE SZ012-NUM-CONTRATO       TO LK-GE3000B-NUM-CONTRATO */
            LkGe3000BParametros.LkGe3000BNumContrato = Convert.ToInt64(DclszContrTerc.Sz012NumContrato);


            /* MOVE 'D'                      TO LK-GE3000B-COD-TP-REGISTRO */
            LkGe3000BParametros.LkGe3000BCodTpRegistro = Utility.PadStrToSize("D", 2);


            /* MOVE FTM01-D-REG-DETALHE      TO LK-GE3000B-TXT-CONTD */
            LkGe3000BParametros.LkGe3000BTxtContd = Utility.PadStrToSize(SerializerUtils.Serialize(Ftm01DRegDetalhe), 2000);


            /* PERFORM PMONITOR-GRAVA-ARQUIVOS */
            Szemb188Pmonitorgravaarquivos();


            /* IF SZ043-IND-ENVIO = 'I'
                          ADD   1                    TO W-QTD-COMPRA1
                       ELSE
                          ADD   1                    TO W-QTD-COMPRA2
                       END-IF */
            if ((Comparisions.SafeStringCompare(DclszObjAcopladoAssist.Sz043IndEnvio, "I")))
            {
                WQtdCompra1 = Math.Abs(Utility.Truncate(WQtdCompra1 + 1));


            }
            else
            {
                WQtdCompra2 = Math.Abs(Utility.Truncate(WQtdCompra2 + 1));



            }


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188P2810gravadetalhe Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188P2820gravatrailler()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188P2820gravatrailler");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE 'P2820'                  TO W-LABEL */
            WLabel = Utility.PadStrToSize("P2820", 5);


            /* MOVE W-QTD-COMPRA1            TO FTM01-T-QTD-INCL */
            Ftm01TRegTrailler.Ftm01TQtdIncl = Math.Abs(Convert.ToInt64(Utility.PadNumToSize(WQtdCompra1, 5)));


            /* MOVE W-QTD-COMPRA2            TO FTM01-T-QTD-ALTE */
            Ftm01TRegTrailler.Ftm01TQtdAlte = Math.Abs(Convert.ToInt64(Utility.PadNumToSize(WQtdCompra2, 5)));


            /* MOVE W-QTD-CANCEL             TO FTM01-T-QTD-CANC */
            Ftm01TRegTrailler.Ftm01TQtdCanc = Math.Abs(Convert.ToInt64(Utility.PadNumToSize(WQtdCancel, 5)));


            /* COMPUTE FTM01-T-QTD-REG = FTM01-T-QTD-INCL
                                                  + FTM01-T-QTD-CANC */
            Ftm01TRegTrailler.Ftm01TQtdReg = Math.Abs(Utility.Truncate(Ftm01TRegTrailler.Ftm01TQtdIncl + Ftm01TRegTrailler.Ftm01TQtdCanc));


            /* WRITE REG-ARQTEMP1 FROM FTM01-T-REG-TRAILLER */
            Arqtemp1.WriteObject(SerializerUtils.Serialize(Ftm01TRegTrailler));


            /* ADD 1                         TO W-SEQ-REGISTRO */
            WSeqRegistro = Math.Abs(Utility.Truncate(WSeqRegistro + 1));


            /* MOVE W-SEQ-REGISTRO           TO LK-GE3000B-NUM-ITEM-MOV */
            LkGe3000BParametros.LkGe3000BNumItemMov = Convert.ToInt64(WSeqRegistro);


            /* INITIALIZE LK-GE3000B-NUM-PES-OPERADOR
                                  LK-GE3000B-NUM-LINHA-PRODUTO
                                  LK-GE3000B-NUM-CONTRATO-TERC
                                  LK-GE3000B-NUM-CONTRATO */
            LkGe3000BParametros.LkGe3000BNumPesOperador = 0;
            LkGe3000BParametros.LkGe3000BNumLinhaProduto = 0;
            LkGe3000BParametros.LkGe3000BNumContratoTerc = 0;
            LkGe3000BParametros.LkGe3000BNumContrato = 0;


            /* MOVE 'T'                      TO LK-GE3000B-COD-TP-REGISTRO */
            LkGe3000BParametros.LkGe3000BCodTpRegistro = Utility.PadStrToSize("T", 2);


            /* MOVE FTM01-T-REG-TRAILLER     TO LK-GE3000B-TXT-CONTD */
            LkGe3000BParametros.LkGe3000BTxtContd = Utility.PadStrToSize(SerializerUtils.Serialize(Ftm01TRegTrailler), 2000);


            /* PERFORM PMONITOR-GRAVA-ARQUIVOS */
            Szemb188Pmonitorgravaarquivos();


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188P2820gravatrailler Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188C0010callspszemnl01()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188C0010callspszemnl01");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* IF H-SZL01-IND-ERRO-LOG = 0
                          DISPLAY '------------------------------------------------'
                            '------------------------------------------------------'
                          DISPLAY W-MENSAGEM-ERRO(001:80)
                          IF W-MENSAGEM-ERRO(081:80) NOT = SPACES
                             DISPLAY W-MENSAGEM-ERRO(081:80)
                          END-IF
                          IF W-MENSAGEM-ERRO(161:80) NOT = SPACES
                             DISPLAY W-MENSAGEM-ERRO(161:80)
                          END-IF
                          IF W-MENSAGEM-ERRO(241:80) NOT = SPACES
                             DISPLAY W-MENSAGEM-ERRO(241:80)
                          END-IF
                          DISPLAY '------------------------------------------------'
                            '------------------------------------------------------'
                       END-IF */
            if (HSzl01Szemnl01.HSzl01IndErroLog == 0)
            {
                Console.WriteLine("------------------------------------------------" + "------------------------------------------------------");


                Console.WriteLine(Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 0, 80));


                if (Utility.CompareWithSpaces(Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 80, 80)) != 0)
                {
                    Console.WriteLine(Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 80, 80));


                }


                if (Utility.CompareWithSpaces(Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 160, 80)) != 0)
                {
                    Console.WriteLine(Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 160, 80));


                }


                if (Utility.CompareWithSpaces(Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 240, 80)) != 0)
                {
                    Console.WriteLine(Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 240, 80));


                }


                Console.WriteLine("------------------------------------------------" + "------------------------------------------------------");


            }


            /* MOVE W-PROGRAMA               TO H-SZL01-COD-USUARIO
                                                       H-SZL01-COD-PROGRAMA */
            HSzl01Szemnl01.HSzl01CodUsuario = Utility.PadStrToSize(WPrograma, 8);
            HSzl01Szemnl01.HSzl01CodPrograma = Utility.PadStrToSize(WPrograma, 10);


            /* INITIALIZE N-SZL01-COD-USUARIO
                                  N-SZL01-COD-PROGRAMA */
            NSzl01Szemnl01.NSzl01CodUsuario = 0;
            NSzl01Szemnl01.NSzl01CodPrograma = 0;


            /* INITIALIZE H-SZL01-DES-MSG-SISTEMA-T
                                  H-SZL01-DES-MSG-SISTEMA-L */
            HSzl01Szemnl01.HSzl01DesMsgSistema.HSzl01DesMsgSistemaT = Utility.PadLeft("", 500, " ");
            HSzl01Szemnl01.HSzl01DesMsgSistema.HSzl01DesMsgSistemaL = 0;


            /* IF H-SZL01-IND-ERRO-LOG = 0
                          STRING W-PROGRAMA '-' W-LABEL '-AVISO: ' W-MENSAGEM-ERRO
                                DELIMITED BY SIZE INTO H-SZL01-DES-MSG-SISTEMA-T
                       ELSE
                          STRING W-PROGRAMA '-' W-LABEL '-ERRO: ' W-MENSAGEM-ERRO
                                DELIMITED BY SIZE INTO H-SZL01-DES-MSG-SISTEMA-T
                       END-IF */
            if (HSzl01Szemnl01.HSzl01IndErroLog == 0)
            {
                StringBuilder builder1 = new StringBuilder();
                builder1.Append(WPrograma).Append("-").Append(WLabel).Append("-AVISO: ").Append(WMensagemErro);
                string builderOut1 = builder1.ToString();
                if (builderOut1.Length > 500)
                {
                    builderOut1 = builderOut1.SafeSubstring(0, 500);
                }
                HSzl01Szemnl01.HSzl01DesMsgSistema.HSzl01DesMsgSistemaT = builderOut1;


            }
            else
            {
                StringBuilder builder2 = new StringBuilder();
                builder2.Append(WPrograma).Append("-").Append(WLabel).Append("-ERRO: ").Append(WMensagemErro);
                string builderOut2 = builder2.ToString();
                if (builderOut2.Length > 500)
                {
                    builderOut2 = builderOut2.SafeSubstring(0, 500);
                }
                HSzl01Szemnl01.HSzl01DesMsgSistema.HSzl01DesMsgSistemaT = builderOut2;



            }


            /* INSPECT FUNCTION REVERSE(H-SZL01-DES-MSG-SISTEMA-T)
                               TALLYING         H-SZL01-DES-MSG-SISTEMA-L
                               FOR LEADING ' ' */
            HSzl01Szemnl01.HSzl01DesMsgSistema.HSzl01DesMsgSistemaL = Utility.GetLeadingCountOfSequence(SerializerUtils.Serialize(Utility.ReverseString(HSzl01Szemnl01.HSzl01DesMsgSistema.HSzl01DesMsgSistemaT)), " ");


            /* INITIALIZE N-SZL01-DES-MSG-SISTEMA */
            NSzl01Szemnl01.NSzl01DesMsgSistema = 0;


            /* IF H-SZL01-IND-ERRO-LOG = 2
                          MOVE SQLCODE               TO H-SZL01-SQLCODE-LOG
                          MOVE SQLERRMC              TO H-SZL01-SQLERRMC-LOG
                       ELSE
                         INITIALIZE H-SZL01-SQLCODE-LOG
                                    H-SZL01-SQLERRMC-LOG
                       END-IF */
            if (HSzl01Szemnl01.HSzl01IndErroLog == 2)
            {
                HSzl01Szemnl01.HSzl01SqlcodeLog = Convert.ToInt64(Sqlca.Sqlcode);


                HSzl01Szemnl01.HSzl01SqlerrmcLog = Utility.PadStrToSize(Sqlca.Sqlerrm.Sqlerrmc, 70);


            }
            else
            {
                HSzl01Szemnl01.HSzl01SqlcodeLog = 0;
                HSzl01Szemnl01.HSzl01SqlerrmcLog = Utility.PadLeft("", 70, " ");



            }


            /* INITIALIZE N-SZL01-SQLCODE-LOG
                                 N-SZL01-SQLERRMC-LOG */
            NSzl01Szemnl01.NSzl01SqlcodeLog = 0;
            NSzl01Szemnl01.NSzl01SqlerrmcLog = 0;


            /* MOVE W-MENSAGEM-ERRO          TO H-SZL01-DES-MSG-RETORNO */
            HSzl01Szemnl01.HSzl01DesMsgRetorno = Utility.PadStrToSize(WMensagemErro, 133);


            /* INITIALIZE N-SZL01-DES-MSG-RETORNO */
            NSzl01Szemnl01.NSzl01DesMsgRetorno = 0;


            /* INITIALIZE H-SZL01-SEQ-LOG-SISTEMA
                                  H-SZL01-IND-ERRO
                                  H-SZL01-MSG-RET
                                  H-SZL01-NM-TAB
                                  H-SZL01-SQLCODE
                                  H-SZL01-SQLERRMC */
            HSzl01Szemnl01.HSzl01SeqLogSistema = 0;
            HSzl01Szemnl01.HSzl01IndErro = 0;
            HSzl01Szemnl01.HSzl01MsgRet = Utility.PadLeft("", 133, " ");
            HSzl01Szemnl01.HSzl01NmTab = Utility.PadLeft("", 30, " ");
            HSzl01Szemnl01.HSzl01Sqlcode = 0;
            HSzl01Szemnl01.HSzl01Sqlerrmc = Utility.PadLeft("", 70, " ");


            /* MOVE -1                       TO N-SZL01-SEQ-LOG-SISTEMA
                                                        N-SZL01-IND-ERRO
                                                        N-SZL01-MSG-RET
                                                        N-SZL01-NM-TAB
                                                        N-SZL01-SQLCODE
                                                        N-SZL01-SQLERRMC */
            NSzl01Szemnl01.NSzl01SeqLogSistema = Convert.ToInt64(-1);
            NSzl01Szemnl01.NSzl01IndErro = Convert.ToInt64(-1);
            NSzl01Szemnl01.NSzl01MsgRet = Convert.ToInt64(-1);
            NSzl01Szemnl01.NSzl01NmTab = Convert.ToInt64(-1);
            NSzl01Szemnl01.NSzl01Sqlcode = Convert.ToInt64(-1);
            NSzl01Szemnl01.NSzl01Sqlerrmc = Convert.ToInt64(-1);


            /* MOVE 'SZEMNL01'               TO W-CALL */
            WCall = Utility.PadStrToSize("SZEMNL01", 8);


            /* *>EXECSQL EXEC SQL
                  *>EXECSQL CALL dbo.SZEMNL01
                  *>EXECSQL ( :H-SZL01-COD-USUARIO
                  *>EXECSQL *>           INDICATOR :N-SZL01-COD-USUARIO
                  *>EXECSQL , :H-SZL01-COD-PROGRAMA
                  *>EXECSQL *>           INDICATOR :N-SZL01-COD-PROGRAMA
                  *>EXECSQL , :H-SZL01-DES-MSG-SISTEMA
                  *>EXECSQL *>           INDICATOR :N-SZL01-DES-MSG-SISTEMA
                  *>EXECSQL , :H-SZL01-IND-ERRO-LOG
                  *>EXECSQL *>           INDICATOR :N-SZL01-IND-ERRO-LOG
                  *>EXECSQL , :H-SZL01-SQLCODE-LOG
                  *>EXECSQL *>           INDICATOR :N-SZL01-SQLCODE-LOG
                  *>EXECSQL , :H-SZL01-SQLERRMC-LOG
                  *>EXECSQL *>           INDICATOR :N-SZL01-SQLERRMC-LOG
                  *>EXECSQL , :H-SZL01-DES-MSG-RETORNO
                  *>EXECSQL *>           INDICATOR :N-SZL01-DES-MSG-RETORNO
                  *>EXECSQL , :H-SZL01-SEQ-LOG-SISTEMA
                  *>EXECSQL *>           INDICATOR :N-SZL01-SEQ-LOG-SISTEMA
                  *>EXECSQL , :H-SZL01-IND-ERRO
                  *>EXECSQL *>           INDICATOR :N-SZL01-IND-ERRO
                  *>EXECSQL , :H-SZL01-MSG-RET
                  *>EXECSQL *>           INDICATOR :N-SZL01-MSG-RET
                  *>EXECSQL , :H-SZL01-NM-TAB
                  *>EXECSQL *>           INDICATOR :N-SZL01-NM-TAB
                  *>EXECSQL , :H-SZL01-SQLCODE
                  *>EXECSQL *>           INDICATOR :N-SZL01-SQLCODE
                  *>EXECSQL , :H-SZL01-SQLERRMC
                  *>EXECSQL *>           INDICATOR :N-SZL01-SQLERRMC
                  *>EXECSQL )
                  *>EXECSQL END-EXEC } */
            // Create the input Object
            NamedQueryInputSzemb188C0010Callspszemnl01 NamedQueryInputSzemb188C0010Callspszemnl01Obj = new NamedQueryInputSzemb188C0010Callspszemnl01();
            NamedQueryInputSzemb188C0010Callspszemnl01Obj.HSzl01CodUsuario = HSzl01Szemnl01.HSzl01CodUsuario.ToString();
            NamedQueryInputSzemb188C0010Callspszemnl01Obj.HSzl01CodPrograma = HSzl01Szemnl01.HSzl01CodPrograma.ToString();
            NamedQueryInputSzemb188C0010Callspszemnl01Obj.HSzl01DesMsgSistema = HSzl01Szemnl01.HSzl01DesMsgSistema.ToString();
            NamedQueryInputSzemb188C0010Callspszemnl01Obj.HSzl01IndErroLog = Convert.ToInt32(HSzl01Szemnl01.HSzl01IndErroLog);
            NamedQueryInputSzemb188C0010Callspszemnl01Obj.HSzl01SqlcodeLog = Convert.ToInt32(HSzl01Szemnl01.HSzl01SqlcodeLog);
            NamedQueryInputSzemb188C0010Callspszemnl01Obj.HSzl01SqlerrmcLog = HSzl01Szemnl01.HSzl01SqlerrmcLog.ToString();
            NamedQueryInputSzemb188C0010Callspszemnl01Obj.HSzl01DesMsgRetorno = HSzl01Szemnl01.HSzl01DesMsgRetorno.ToString();
            NamedQueryInputSzemb188C0010Callspszemnl01Obj.HSzl01SeqLogSistema = Convert.ToInt64(HSzl01Szemnl01.HSzl01SeqLogSistema);
            NamedQueryInputSzemb188C0010Callspszemnl01Obj.HSzl01IndErro = Convert.ToInt32(HSzl01Szemnl01.HSzl01IndErro);
            NamedQueryInputSzemb188C0010Callspszemnl01Obj.HSzl01MsgRet = HSzl01Szemnl01.HSzl01MsgRet.ToString();
            NamedQueryInputSzemb188C0010Callspszemnl01Obj.HSzl01NmTab = HSzl01Szemnl01.HSzl01NmTab.ToString();
            NamedQueryInputSzemb188C0010Callspszemnl01Obj.HSzl01Sqlcode = Convert.ToInt32(HSzl01Szemnl01.HSzl01Sqlcode);
            NamedQueryInputSzemb188C0010Callspszemnl01Obj.HSzl01Sqlerrmc = HSzl01Szemnl01.HSzl01Sqlerrmc.ToString();

            // Execute the Call Statement
            NHibernateUtilSzemb188 nhUtil = new NHibernateUtilSzemb188(Sqlca);
            nhUtil.ExecuteCallNamedQuerySzemb188C0010Callspszemnl01(NamedQueryInputSzemb188C0010Callspszemnl01Obj);

            // Get the output variables
            HSzl01Szemnl01.HSzl01DesMsgRetorno = NamedQueryInputSzemb188C0010Callspszemnl01Obj.HSzl01DesMsgRetorno;
            HSzl01Szemnl01.HSzl01SeqLogSistema = NamedQueryInputSzemb188C0010Callspszemnl01Obj.HSzl01SeqLogSistema;
            HSzl01Szemnl01.HSzl01IndErro = NamedQueryInputSzemb188C0010Callspszemnl01Obj.HSzl01IndErro;
            HSzl01Szemnl01.HSzl01MsgRet = NamedQueryInputSzemb188C0010Callspszemnl01Obj.HSzl01MsgRet;
            HSzl01Szemnl01.HSzl01NmTab = NamedQueryInputSzemb188C0010Callspszemnl01Obj.HSzl01NmTab;
            HSzl01Szemnl01.HSzl01Sqlcode = NamedQueryInputSzemb188C0010Callspszemnl01Obj.HSzl01Sqlcode;
            HSzl01Szemnl01.HSzl01Sqlerrmc = NamedQueryInputSzemb188C0010Callspszemnl01Obj.HSzl01Sqlerrmc;


            /* IF SQLCODE NOT = 000
                          DISPLAY ' '
                          DISPLAY '################################################'
                                '##################################################'
                          DISPLAY 'ERRO CALL DA SP ' W-CALL '.'
                          DISPLAY '################################################'
                                '##################################################'
                          MOVE SQLCODE               TO E-SQLCODE
                          DISPLAY 'SQLCODE            =' E-SQLCODE
                          MOVE SQLERRD(3)            TO E-SQLCODE
                          DISPLAY 'SQLERRD(3)         =' E-SQLCODE
                          IF SQLERRMC NOT = SPACES
                             DISPLAY 'SQLERRMC=<'   SQLERRMC '>'
                          END-IF
                          DISPLAY '################################################'
                                '##################################################'
                       ELSE
                  *>        --- VERIFICA SE A PROCEDURE RETORNOU ERRO
                          IF H-SZL01-IND-ERRO NOT = 000
                             DISPLAY '#############################################'
                             '#####################################################'
                             DISPLAY 'ERRO NO RETORNO DA SP ' W-CALL '.'
                             DISPLAY '#############################################'
                             '#####################################################'
                             MOVE H-SZL01-SQLCODE    TO E-SQLCODE
                             MOVE H-SZL01-IND-ERRO   TO E-SMALLINT-1
                             DISPLAY ' SZL01-IND-ERRO='  E-SMALLINT-1
                             DISPLAY ' SZL01-SQLCODE = ' E-SQLCODE
                             DISPLAY ' SZL01-NM-TAB  ='  H-SZL01-NM-TAB
                             DISPLAY ' SZL01-SQLERRMC= ' H-SZL01-SQLERRMC
                             DISPLAY '#############################################'
                             '#####################################################'
                          END-IF
                       END-IF */
            if (Sqlca.Sqlcode != 000)
            {
                Console.WriteLine(" ");


                Console.WriteLine("################################################" + "##################################################");


                Console.WriteLine("ERRO CALL DA SP " + WCall + ".");


                Console.WriteLine("################################################" + "##################################################");


                ESqlcode = Convert.ToInt64(Utility.PadNumToSize(Sqlca.Sqlcode, 4));


                Console.WriteLine("SQLCODE            =" + ESqlcode);


                ESqlcode = Utility.PadNumToSize(Sqlca.Sqlerrd[2], 4);


                Console.WriteLine("SQLERRD(3)         =" + ESqlcode);


                if (Utility.CompareWithSpaces(Sqlca.Sqlerrm.Sqlerrmc) != 0)
                {
                    Console.WriteLine("SQLERRMC=<" + Sqlca.Sqlerrm.Sqlerrmc + ">");


                }


                Console.WriteLine("################################################" + "##################################################");


            }
            else
            {
                if (HSzl01Szemnl01.HSzl01IndErro != 000)
                {
                    Console.WriteLine("#############################################" + "#####################################################");


                    Console.WriteLine("ERRO NO RETORNO DA SP " + WCall + ".");


                    Console.WriteLine("#############################################" + "#####################################################");


                    ESqlcode = Convert.ToInt64(Utility.PadNumToSize(HSzl01Szemnl01.HSzl01Sqlcode, 4));


                    ESmallint1 = Math.Abs(Convert.ToInt64(HSzl01Szemnl01.HSzl01IndErro));


                    Console.WriteLine(" SZL01-IND-ERRO=" + ESmallint1);


                    Console.WriteLine(" SZL01-SQLCODE = " + ESqlcode);


                    Console.WriteLine(" SZL01-NM-TAB  =" + HSzl01Szemnl01.HSzl01NmTab);


                    Console.WriteLine(" SZL01-SQLERRMC= " + HSzl01Szemnl01.HSzl01Sqlerrmc);


                    Console.WriteLine("#############################################" + "#####################################################");


                }



            }


            /* IF N-SZL01-SEQ-LOG-SISTEMA = -1
                          INITIALIZE H-SZL01-SEQ-LOG-SISTEMA
                       END-IF */
            if (NSzl01Szemnl01.NSzl01SeqLogSistema == -1)
            {
                HSzl01Szemnl01.HSzl01SeqLogSistema = 0;


            }


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188C0010callspszemnl01 Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188Db010opencrcertcompra()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188Db010opencrcertcompra");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE 'DB010'                  TO W-LABEL */
            WLabel = Utility.PadStrToSize("DB010", 5);


            /* *>EXECSQL EXEC SQL DECLARE CR_CERTIF_COMPRA CURSOR FOR
                  *>EXECSQL SELECT VALUE(SZ012.NUM_CONTRATO_TERC,0) AS NUM_CONTR_TERC
                  *>EXECSQL , SZ012.DTA_INI_VIG_TERC + 1 MONTH AS DTA_PROX_COBRANC
                  *>EXECSQL , SZ011.COD_PRODUTO
                  *>EXECSQL , VALUE(SZ098.NUM_CONTRATO_FORN,' ') AS NUM_CONTR_FORN
                  *>EXECSQL , '0'                              AS NUM_VERSAO_SERIE
                  *>EXECSQL , VALUE(SZ098.COD_GCS_FORN,' ') AS COD_GCS_FORN
                  *>EXECSQL , SZ012.DTA_ASSINATURA
                  *>EXECSQL , VALUE(SZ012.DTA_FIM_VIG_TERC,'9999-12-31')
                  *>EXECSQL AS DTH_FIM_CONTRATO
                  *>EXECSQL , VALUE(SZ021.QTD_MESES_CONTRATO, 12)
                  *>EXECSQL AS QTD_MESES_CONTRATO
                  *>EXECSQL , SZ012.NUM_CONTRATO
                  *>EXECSQL , SZ010.NUM_APOLICE
                  *>EXECSQL , SZ011.NUM_ORI_CONTRATO
                  *>EXECSQL , SZ043.DTA_ACIONAMENTO         AS DTA_FIM_CARENCIA
                  *>EXECSQL , SZ012.NUM_PES_OPERADOR
                  *>EXECSQL , SZ012.NUM_LINHA_PRODUTO
                  *>EXECSQL , SZ012.NUM_CONTRATO_ANT
                  *>EXECSQL , SZ012.STA_CONTRATO_TERC
                  *>EXECSQL , SZ072.COD_ACOPLADO
                  *>EXECSQL , IFNULL(SZ098.COD_PROD_ACOPLADO, 0)
                  *>EXECSQL , IFNULL(SZ098.SEQ_ENVIO, 0) + 1   AS SEQ_ENVIO
                  *>EXECSQL , SZ115.SEQ_ACOPLADO
                  *>EXECSQL , SZ115.DTA_INI_VIGENCIA
                  *>EXECSQL , SZ115.DTA_FIM_VIGENCIA
                  *>EXECSQL , SZ043.IND_ENVIO
                  *>EXECSQL , SZ021.DTA_INI_VIG_SEG
                  *>EXECSQL , SZ021.DTA_FIM_VIG_SEG
                  *>EXECSQL FROM dbo.SZ_ORIGEM_CONTRATO SZ011
                  *>EXECSQL JOIN dbo.SZ_APOLICE         SZ010
                  *>EXECSQL ON SZ010.SEQ_PROP_APOL      = SZ011.SEQ_APOLICE
                  *>EXECSQL JOIN dbo.SZ_CONTR_TERC      SZ012
                  *>EXECSQL ON SZ012.NUM_ORI_CONTRATO   = SZ011.NUM_ORI_CONTRATO
                  *>EXECSQL AND SZ012.STA_CONTRATO_TERC  = 'A'
                  *>EXECSQL JOIN dbo.SZ_CONTR_SEGURO    SZ021
                  *>EXECSQL ON SZ021.NUM_CONTRATO       = SZ012.NUM_CONTRATO
                  *>EXECSQL JOIN dbo.SZ_APOL_ACOPLADO   SZ073
                  *>EXECSQL ON SZ073.SEQ_APOLICE        = SZ011.SEQ_APOLICE
                  *>EXECSQL AND (CURRENT DATE BETWEEN SZ073.DTA_INI_VIGENCIA
                  *>EXECSQL AND VALUE(SZ073.DTA_FIM_VIGENCIA,'2999-12-31'))
                  *>EXECSQL JOIN dbo.SZ_ACOPLADO        SZ072
                  *>EXECSQL ON SZ072.COD_ACOPLADO       = SZ073.COD_ACOPLADO
                  *>EXECSQL AND STA_ACOPLADO             = 'A'
                  *>EXECSQL AND SZ072.COD_ACOPLADO       = 11
                  *>EXECSQL AND SZ072.COD_TP_ACOPLADO    = 2
                  *>EXECSQL JOIN dbo.SZ_ACOPLADO_ASSIST SZ098
                  *>EXECSQL ON SZ098.COD_ACOPLADO       = SZ072.COD_ACOPLADO
                  *>EXECSQL JOIN dbo.SZ_OBJ_ACOPLADO    SZ115
                  *>EXECSQL ON SZ115.NUM_CONTRATO       = SZ012.NUM_CONTRATO
                  *>EXECSQL AND SZ115.STA_ENVIO          = 'E'
                  *>EXECSQL AND SZ115.COD_ACOPLADO       = SZ072.COD_ACOPLADO
                  *>EXECSQL AND SZ115.DTA_VINCULACAO BETWEEN :WS-DT-INI
                  *>EXECSQL AND :WS-DT-FIM
                  *>EXECSQL JOIN dbo.SZ_OBJ_ACOPLADO_ASSIST SZ043
                  *>EXECSQL ON SZ043.NUM_CONTRATO       = SZ115.NUM_CONTRATO
                  *>EXECSQL AND SZ043.COD_ACOPLADO       = SZ115.COD_ACOPLADO
                  *>EXECSQL AND SZ043.SEQ_ACOPLADO       = SZ115.SEQ_ACOPLADO
                  *>EXECSQL WHERE SZ011.COD_PRODUTO       BETWEEN :H-COD-PRODUTO-MIN
                  *>EXECSQL AND :H-COD-PRODUTO-MAX
                  *>EXECSQL AND SZ072.COD_ACOPLADO      BETWEEN :H-COD-ACOPLADO-MIN
                  *>EXECSQL AND :H-COD-ACOPLADO-MAX
                  *>EXECSQL AND CURRENT_DATE BETWEEN SZ012.DTA_INI_VIG_TERC
                  *>EXECSQL AND SZ012.DTA_FIM_VIG_TERC
                  *>EXECSQL ORDER BY SZ012.NUM_CONTRATO, SZ012.DTA_INI_VIG_TERC
                  *>EXECSQL
                  *>EXECSQL END-EXEC } */


            /* DISPLAY ' ' */
            Console.WriteLine(" ");


            /* DISPLAY W-PROGRAMA '- VAI ABRIR CURSOR CR_CERTIF_COMPRA COM:'
                                           ' TIME=' FUNCTION CURRENT-DATE */
            Console.WriteLine(WPrograma + "- VAI ABRIR CURSOR CR_CERTIF_COMPRA COM:" + " TIME=" + Utility.GetCurrentDate());


            /* DISPLAY ' COD-PRODUTO INICIO  = ' H-COD-PRODUTO-MIN */
            Console.WriteLine(" COD-PRODUTO INICIO  = " + HCodProdutoMin);


            /* DISPLAY ' COD-PRODUTO FIM     = ' H-COD-PRODUTO-MAX */
            Console.WriteLine(" COD-PRODUTO FIM     = " + HCodProdutoMax);


            /* DISPLAY ' COD-ACOPLADO INICIO = ' H-COD-ACOPLADO-MIN */
            Console.WriteLine(" COD-ACOPLADO INICIO = " + HCodAcopladoMin);


            /* DISPLAY ' COD-ACOPLADO FIM    = ' H-COD-ACOPLADO-MAX */
            Console.WriteLine(" COD-ACOPLADO FIM    = " + HCodAcopladoMax);


            /* DISPLAY ' ' */
            Console.WriteLine(" ");


            /* *>EXECSQL EXEC SQL
                  *>EXECSQL OPEN  CR_CERTIF_COMPRA
                  *>EXECSQL END-EXEC } */
            // Create the input Object
            NamedQueryInputSzemb188CrCertifCompra NamedQueryInputSzemb188CrCertifCompraObj = new NamedQueryInputSzemb188CrCertifCompra();
            NamedQueryInputSzemb188CrCertifCompraObj.WsDtIni = Utility.ParseDateTime(WsDtIni);
            NamedQueryInputSzemb188CrCertifCompraObj.WsDtFim = Utility.ParseDateTime(WsDtFim);
            NamedQueryInputSzemb188CrCertifCompraObj.HCodProdutoMin = Convert.ToInt32(HCodProdutoMin);
            NamedQueryInputSzemb188CrCertifCompraObj.HCodProdutoMax = Convert.ToInt32(HCodProdutoMax);
            NamedQueryInputSzemb188CrCertifCompraObj.HCodAcopladoMin = Convert.ToInt32(HCodAcopladoMin);
            NamedQueryInputSzemb188CrCertifCompraObj.HCodAcopladoMax = Convert.ToInt32(HCodAcopladoMax);

            // Execute the NamedQuery
            NHibernateUtilSzemb188 nhUtilCursorSzemb188CrCertifCompra = new NHibernateUtilSzemb188(Sqlca);
            szemb188CursorCrCertifCompra = nhUtilCursorSzemb188CrCertifCompra.ExecuteSelectNamedQuerySzemb188CrCertifCompra(NamedQueryInputSzemb188CrCertifCompraObj);


            /* DISPLAY ' ' */
            Console.WriteLine(" ");


            /* DISPLAY W-PROGRAMA '-ABRIU     CURSOR CR_CERTIF_COMPRA AS '
                               FUNCTION CURRENT-DATE */
            Console.WriteLine(WPrograma + "-ABRIU     CURSOR CR_CERTIF_COMPRA AS " + Utility.GetCurrentDate());


            /* IF SQLCODE NOT EQUAL ZEROS
                          INITIALIZE W-MENSAGEM-ERRO
                          STRING 'OPEN CURSOR CR_CERTIF_COMPRA.'
                                 ' COD-PRODUTO  BETWEEN '          E-INTEGER-1
                                                  ' AND '          E-INTEGER-2
                                 ' COD-ACOPLADO BETWEEN '          E-INTEGER-3
                                                  ' AND '          E-INTEGER-4
                                 DELIMITED BY SIZE INTO W-MENSAGEM-ERRO
                          GO TO P9990-DB2-ERROR
                       END-IF */
            if (Sqlca.Sqlcode != 0)
            {
                WMensagemErro = Utility.PadLeft("", 500, " ");


                StringBuilder builder1 = new StringBuilder();
                builder1.Append("OPEN CURSOR CR_CERTIF_COMPRA.").Append(" COD-PRODUTO  BETWEEN ").Append(SerializerUtils.Serialize(EInteger1, (DataTypeMetadata)DataTypeMap["EInteger1"])).Append(" AND ").Append(SerializerUtils.Serialize(EInteger2, (DataTypeMetadata)DataTypeMap["EInteger2"])).Append(" COD-ACOPLADO BETWEEN ").Append(SerializerUtils.Serialize(EInteger3, (DataTypeMetadata)DataTypeMap["EInteger3"])).Append(" AND ").Append(SerializerUtils.Serialize(EInteger4, (DataTypeMetadata)DataTypeMap["EInteger4"]));
                string builderOut1 = builder1.ToString();
                if (builderOut1.Length > 500)
                {
                    builderOut1 = builderOut1.SafeSubstring(0, 500);
                }
                WMensagemErro = builderOut1;


                Szemb188P9990db2error();


            }


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188Db010opencrcertcompra Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188Db020fetchcertcompra()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188Db020fetchcertcompra");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE 'DB020'                  TO W-LABEL */
            WLabel = Utility.PadStrToSize("DB020", 5);


            /* INITIALIZE SZ012-NUM-CONTRATO-TERC
                                  SZ098-SEQ-ENVIO
                                  SZ012-NUM-CONTRATO-ANT */
            DclszContrTerc.Sz012NumContratoTerc = 0;
            DclszAcopladoAssist.Sz098SeqEnvio = 0;
            DclszContrTerc.Sz012NumContratoAnt = 0;


            /* MOVE -1                       TO N-SZ012-NUM-CONTRATO-ANT */
            NSz012NumContratoAnt = Convert.ToInt64(-1);


            /* *>EXECSQL EXEC SQL
                  *>EXECSQL FETCH CR_CERTIF_COMPRA
                  *>EXECSQL INTO   :SZ012-NUM-CONTRATO-TERC
                  *>EXECSQL ,:SZ012-DTA-INI-VIG-TERC
                  *>EXECSQL ,:SZ011-COD-PRODUTO
                  *>EXECSQL ,:SZ098-NUM-CONTRATO-FORN
                  *>EXECSQL ,:H-NUM-VERSAO-SERIE
                  *>EXECSQL ,:SZ098-COD-GCS-FORN
                  *>EXECSQL ,:SZ012-DTA-ASSINATURA
                  *>EXECSQL ,:H-DTA-INI-VIGENCIA-ATUAL
                  *>EXECSQL ,:SZ021-QTD-MESES-CONTRATO
                  *>EXECSQL ,:SZ012-NUM-CONTRATO
                  *>EXECSQL ,:SZ010-NUM-APOLICE
                  *>EXECSQL ,:SZ011-NUM-ORI-CONTRATO
                  *>EXECSQL ,:SZ043-DTA-ACIONAMENTO
                  *>EXECSQL ,:SZ012-NUM-PES-OPERADOR
                  *>EXECSQL ,:SZ012-NUM-LINHA-PRODUTO
                  *>EXECSQL ,:SZ012-NUM-CONTRATO-ANT
                  *>EXECSQL *>                 INDICATOR :N-SZ012-NUM-CONTRATO-ANT
                  *>EXECSQL ,:SZ012-STA-CONTRATO-TERC
                  *>EXECSQL ,:SZ072-COD-ACOPLADO
                  *>EXECSQL ,:SZ098-COD-PROD-ACOPLADO
                  *>EXECSQL ,:SZ098-SEQ-ENVIO
                  *>EXECSQL ,:SZ115-SEQ-ACOPLADO
                  *>EXECSQL ,:SZ115-DTA-INI-VIGENCIA
                  *>EXECSQL ,:SZ115-DTA-FIM-VIGENCIA
                  *>EXECSQL ,:SZ043-IND-ENVIO
                  *>EXECSQL ,:SZ021-DTA-INI-VIG-SEG
                  *>EXECSQL ,:SZ021-DTA-FIM-VIG-SEG
                  *>EXECSQL END-EXEC } */
            NamedQueryResponseSzemb188CrCertifCompra NamedQueryResponseSzemb188CrCertifCompraResp = szemb188CursorCrCertifCompra.Fetch();
            if (!szemb188CursorCrCertifCompra.IsEOF())
            {
                DclszContrTerc.Sz012NumContratoTerc = Convert.ToInt64(NamedQueryResponseSzemb188CrCertifCompraResp.NumContrTerc);
                DclszContrTerc.Sz012DtaIniVigTerc = NamedQueryResponseSzemb188CrCertifCompraResp.DtaProxCobranc;
                DclszOrigemContrato.Sz011CodProduto = Convert.ToInt64(NamedQueryResponseSzemb188CrCertifCompraResp.CodProduto);
                DclszAcopladoAssist.Sz098NumContratoForn = NamedQueryResponseSzemb188CrCertifCompraResp.NumContrForn;
                HNumVersaoSerie = Convert.ToInt64(NamedQueryResponseSzemb188CrCertifCompraResp.NumVersaoSerie);
                DclszAcopladoAssist.Sz098CodGcsForn = NamedQueryResponseSzemb188CrCertifCompraResp.CodGcsForn;
                DclszContrTerc.Sz012DtaAssinatura = NamedQueryResponseSzemb188CrCertifCompraResp.DtaAssinatura.ToString(Utility.DEFAULT_DATE_FORMAT);
                HDtaIniVigenciaAtual = NamedQueryResponseSzemb188CrCertifCompraResp.DthFimContrato;
                DclszContrSeguro.Sz021QtdMesesContrato = Convert.ToInt64(NamedQueryResponseSzemb188CrCertifCompraResp.QtdMesesContrato);
                DclszContrTerc.Sz012NumContrato = Convert.ToInt64(NamedQueryResponseSzemb188CrCertifCompraResp.NumContrato);
                DclszApolice.Sz010NumApolice = Convert.ToInt64(NamedQueryResponseSzemb188CrCertifCompraResp.NumApolice);
                DclszOrigemContrato.Sz011NumOriContrato = Convert.ToInt64(NamedQueryResponseSzemb188CrCertifCompraResp.NumOriContrato);
                DclszObjAcopladoAssist.Sz043DtaAcionamento = NamedQueryResponseSzemb188CrCertifCompraResp.DtaFimCarencia.ToString(Utility.DEFAULT_DATE_FORMAT);
                DclszContrTerc.Sz012NumPesOperador = Convert.ToInt64(NamedQueryResponseSzemb188CrCertifCompraResp.NumPesOperador);
                DclszContrTerc.Sz012NumLinhaProduto = Convert.ToInt64(NamedQueryResponseSzemb188CrCertifCompraResp.NumLinhaProduto);
                DclszContrTerc.Sz012NumContratoAnt = Convert.ToInt64(NamedQueryResponseSzemb188CrCertifCompraResp.NumContratoAnt);
                DclszContrTerc.Sz012StaContratoTerc = NamedQueryResponseSzemb188CrCertifCompraResp.StaContratoTerc;
                DclszAcoplado.Sz072CodAcoplado = Convert.ToInt64(NamedQueryResponseSzemb188CrCertifCompraResp.CodAcoplado);
                DclszAcopladoAssist.Sz098CodProdAcoplado = Convert.ToInt64(NamedQueryResponseSzemb188CrCertifCompraResp.IfnullSz098CodProdA8861Be);
                DclszAcopladoAssist.Sz098SeqEnvio = Convert.ToInt64(NamedQueryResponseSzemb188CrCertifCompraResp.SeqEnvio);
                DclszObjAcoplado.Sz115SeqAcoplado = Convert.ToInt64(NamedQueryResponseSzemb188CrCertifCompraResp.SeqAcoplado);
                DclszObjAcoplado.Sz115DtaIniVigencia = NamedQueryResponseSzemb188CrCertifCompraResp.DtaIniVigencia.ToString(Utility.DEFAULT_DATE_FORMAT);
                DclszObjAcoplado.Sz115DtaFimVigencia = NamedQueryResponseSzemb188CrCertifCompraResp.DtaFimVigencia.ToString(Utility.DEFAULT_DATE_FORMAT);
                DclszObjAcopladoAssist.Sz043IndEnvio = NamedQueryResponseSzemb188CrCertifCompraResp.IndEnvio;
                DclszContrSeguro.Sz021DtaIniVigSeg = NamedQueryResponseSzemb188CrCertifCompraResp.DtaIniVigSeg.ToString(Utility.DEFAULT_DATE_FORMAT);
                DclszContrSeguro.Sz021DtaFimVigSeg = NamedQueryResponseSzemb188CrCertifCompraResp.DtaFimVigSeg.ToString(Utility.DEFAULT_DATE_FORMAT);
            }


            /* EVALUATE SQLCODE
                       WHEN +100
                         MOVE 'SIM'                  TO W-FIM-CERTIF
                  *>EXECSQL EXEC SQL CLOSE CR_CERTIF_COMPRA END-EXEC }
                       WHEN 000
                         ADD 1                       TO W-TOT-LIDOS-CERT-COMP
                         IF W-TOT-LIDOS-CERT-COMP > 100
                            INITIALIZE SZEMB188-TRACE
                         END-IF
                         ADD 1                       TO W-QTD-DISPLAY-TRACE
                         IF W-QTD-DISPLAY-TRACE = 1000
                            MOVE SZEMB188-TRACE      TO W-SZEMB188-TRACE
                            MOVE 'TRACE ON'          TO SZEMB188-TRACE
                         END-IF
                         IF W-QTD-DISPLAY-TRACE = 1001
                            MOVE W-SZEMB188-TRACE    TO SZEMB188-TRACE
                            MOVE 1                   TO W-QTD-DISPLAY-TRACE
                         END-IF

                         IF SZEMB188-TRACE-ON-88
                            DISPLAY 'DB020-FETCH (' W-TOT-LIDOS-CERT-COMP ')'
                                   ' CONTTER='      SZ012-NUM-CONTRATO-TERC
                                   ' DTAINIVIGTER=' SZ012-DTA-INI-VIG-TERC
                                   ' PRODUTO   ='   SZ011-COD-PRODUTO
                                   ' CONTFORN='     SZ098-NUM-CONTRATO-FORN
                                   ' VERSER='       H-NUM-VERSAO-SERIE
                                   ' GCSFORN='      SZ098-COD-GCS-FORN
                                   ' DTAASSI='      SZ012-DTA-ASSINATURA
                                   ' INIVIGATUAL='  H-DTA-INI-VIGENCIA-ATUAL
                                   ' QTDMESCONT='   SZ021-QTD-MESES-CONTRATO
                                   ' CONT='         SZ012-NUM-CONTRATO
                                   ' APOL='         SZ010-NUM-APOLICE
                                   ' ORICONT='      SZ011-NUM-ORI-CONTRATO
                                   ' PESOPER='      SZ012-NUM-PES-OPERADOR
                                   ' LINPROD='      SZ012-NUM-LINHA-PRODUTO
                                   ' CONTANT='      N-SZ012-NUM-CONTRATO-ANT '/'
                                                    SZ012-NUM-CONTRATO-ANT
                                   ' STACONTTER='   SZ012-STA-CONTRATO-TERC
                                   ' ACOPL='        SZ072-COD-ACOPLADO
                                   ' PRODACOPL='    SZ098-COD-PROD-ACOPLADO
                                   ' SEQENV='       SZ098-SEQ-ENVIO
                                   ' DTA INI='  SZ115-DTA-INI-VIGENCIA
                                   ' DTA FIM='  SZ115-DTA-FIM-VIGENCIA
                                   ' TIME=' FUNCTION CURRENT-DATE
                            DISPLAY 'TOTAIS SAF/CESTA: QTD COMPRA=' W-QTD-COMPRA1
                                    ' QTD RE-COMPRA=' W-QTD-COMPRA2
                         END-IF

                         MOVE SZ012-NUM-CONTRATO-TERC
                                                     TO W-NUM-CONTRATO-TERC
                         MOVE SZ012-NUM-CONTRATO-ANT TO W-NUM-CONTRATO-ANT
                         MOVE SZ012-NUM-CONTRATO     TO W-NUM-CONTRATO
                         IF N-SZ012-NUM-CONTRATO-ANT = -1
                         OR SZ012-NUM-CONTRATO-ANT = SZ012-NUM-CONTRATO-TERC
                            MOVE SZ012-NUM-CONTRATO-TERC
                                                     TO W-NUM-CONTR-APOLICE
                         ELSE
                            MOVE SZ012-NUM-CONTRATO-ANT
                                                     TO W-NUM-CONTR-APOLICE
                         END-IF
                       WHEN OTHER
                         MOVE H-COD-PRODUTO-MIN      TO E-INTEGER-1
                         MOVE H-COD-PRODUTO-MAX      TO E-INTEGER-2
                         MOVE H-COD-ACOPLADO-MIN     TO E-INTEGER-3
                         MOVE H-COD-ACOPLADO-MAX     TO E-INTEGER-4
                         INITIALIZE W-MENSAGEM-ERRO
                         STRING 'FETCH CURSOR CR_CERTIF_COMPRA.'
                                ' COD-PRODUTO BETWEEN '           E-INTEGER-1
                                                 'AND '           E-INTEGER-2
                                ' COD-ACOPLADO BETWEEN '          E-INTEGER-3
                                                 ' AND '          E-INTEGER-4
                                DELIMITED BY SIZE INTO W-MENSAGEM-ERRO
                         GO TO P9990-DB2-ERROR
                       END-EVALUATE */
            if (Sqlca.Sqlcode == +100)
            {
                WFimCertif = Utility.PadStrToSize("SIM", 3);


                szemb188CursorCrCertifCompra.Close();


            }
            else if (Sqlca.Sqlcode == 000)
            {
                WTotLidosCertComp = Math.Abs(Utility.Truncate(WTotLidosCertComp + 1));


                if (WTotLidosCertComp > 100)
                {
                    Szemb188Parametros.Szemb188Trace = Utility.PadLeft("", 8, " ");


                }


                WQtdDisplayTrace = Math.Abs(Utility.Truncate(WQtdDisplayTrace + 1));


                if (WQtdDisplayTrace == 1000)
                {
                    WSzemb188Trace = Utility.PadStrToSize(Szemb188Parametros.Szemb188Trace, 8);


                    Szemb188Parametros.Szemb188Trace = Utility.PadStrToSize("TRACE ON", 8);


                }


                if (WQtdDisplayTrace == 1001)
                {
                    Szemb188Parametros.Szemb188Trace = Utility.PadStrToSize(WSzemb188Trace, 8);


                    WQtdDisplayTrace = Math.Abs(Convert.ToInt64(1));


                }


                if (Szemb188Parametros.Szemb188Szemb188traceon88())
                {
                    Console.WriteLine("DB020-FETCH (" + WTotLidosCertComp + ")" + " CONTTER=" + DclszContrTerc.Sz012NumContratoTerc + " DTAINIVIGTER=" + DclszContrTerc.Sz012DtaIniVigTerc + " PRODUTO   =" + DclszOrigemContrato.Sz011CodProduto + " CONTFORN=" + DclszAcopladoAssist.Sz098NumContratoForn + " VERSER=" + HNumVersaoSerie + " GCSFORN=" + DclszAcopladoAssist.Sz098CodGcsForn + " DTAASSI=" + DclszContrTerc.Sz012DtaAssinatura + " INIVIGATUAL=" + HDtaIniVigenciaAtual + " QTDMESCONT=" + DclszContrSeguro.Sz021QtdMesesContrato + " CONT=" + DclszContrTerc.Sz012NumContrato + " APOL=" + DclszApolice.Sz010NumApolice + " ORICONT=" + DclszOrigemContrato.Sz011NumOriContrato + " PESOPER=" + DclszContrTerc.Sz012NumPesOperador + " LINPROD=" + DclszContrTerc.Sz012NumLinhaProduto + " CONTANT=" + NSz012NumContratoAnt + "/" + DclszContrTerc.Sz012NumContratoAnt + " STACONTTER=" + DclszContrTerc.Sz012StaContratoTerc + " ACOPL=" + DclszAcoplado.Sz072CodAcoplado + " PRODACOPL=" + DclszAcopladoAssist.Sz098CodProdAcoplado + " SEQENV=" + DclszAcopladoAssist.Sz098SeqEnvio + " DTA INI=" + DclszObjAcoplado.Sz115DtaIniVigencia + " DTA FIM=" + DclszObjAcoplado.Sz115DtaFimVigencia + " TIME=" + Utility.GetCurrentDate());


                    Console.WriteLine("TOTAIS SAF/CESTA: QTD COMPRA=" + WQtdCompra1 + " QTD RE-COMPRA=" + WQtdCompra2);


                }


                WNumContratoTerc = Math.Abs(Convert.ToInt64(DclszContrTerc.Sz012NumContratoTerc));


                WNumContratoAnt = Math.Abs(Convert.ToInt64(DclszContrTerc.Sz012NumContratoAnt));


                WNumContrato = Math.Abs(Convert.ToInt64(DclszContrTerc.Sz012NumContrato));


                if (NSz012NumContratoAnt == -1 || DclszContrTerc.Sz012NumContratoAnt == DclszContrTerc.Sz012NumContratoTerc)
                {
                    WNumContrApolice = Math.Abs(Convert.ToInt64(DclszContrTerc.Sz012NumContratoTerc));


                }
                else
                {
                    WNumContrApolice = Math.Abs(Convert.ToInt64(DclszContrTerc.Sz012NumContratoAnt));



                }


            }
            else
            {
                EInteger1 = Math.Abs(Convert.ToInt64(HCodProdutoMin));


                EInteger2 = Math.Abs(Convert.ToInt64(HCodProdutoMax));


                EInteger3 = Math.Abs(Convert.ToInt64(HCodAcopladoMin));


                EInteger4 = Math.Abs(Convert.ToInt64(HCodAcopladoMax));


                WMensagemErro = Utility.PadLeft("", 500, " ");


                StringBuilder builder1 = new StringBuilder();
                builder1.Append("FETCH CURSOR CR_CERTIF_COMPRA.").Append(" COD-PRODUTO BETWEEN ").Append(SerializerUtils.Serialize(EInteger1, (DataTypeMetadata)DataTypeMap["EInteger1"])).Append("AND ").Append(SerializerUtils.Serialize(EInteger2, (DataTypeMetadata)DataTypeMap["EInteger2"])).Append(" COD-ACOPLADO BETWEEN ").Append(SerializerUtils.Serialize(EInteger3, (DataTypeMetadata)DataTypeMap["EInteger3"])).Append(" AND ").Append(SerializerUtils.Serialize(EInteger4, (DataTypeMetadata)DataTypeMap["EInteger4"]));
                string builderOut1 = builder1.ToString();
                if (builderOut1.Length > 500)
                {
                    builderOut1 = builderOut1.SafeSubstring(0, 500);
                }
                WMensagemErro = builderOut1;


                Szemb188P9990db2error();


            }


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188Db020fetchcertcompra Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188Db030abrirpactuante()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188Db030abrirpactuante");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE 'DB030'                  TO W-LABEL */
            WLabel = Utility.PadStrToSize("DB030", 5);


            /* MOVE SPACES                   TO W-FIM-PACTUANTE */
            WFimPactuante = Utility.PadStrToSize(" ", 3);


            /* *>EXECSQL EXEC SQL
                  *>EXECSQL OPEN CR_PACTUANTE
                  *>EXECSQL END-EXEC } */
            // Create the input Object
            NamedQueryInputSzemb188Crpactuante NamedQueryInputSzemb188CrpactuanteObj = new NamedQueryInputSzemb188Crpactuante();
            NamedQueryInputSzemb188CrpactuanteObj.Sz012NumContratoTerc = Convert.ToInt64(DclszContrTerc.Sz012NumContratoTerc);

            // Execute the NamedQuery
            NHibernateUtilSzemb188 nhUtilCursorszemb188CrPactuante = new NHibernateUtilSzemb188(Sqlca);
            szemb188CursorCrPactuante = nhUtilCursorszemb188CrPactuante.ExecuteSelectNamedQuerySzemb188Crpactuante(NamedQueryInputSzemb188CrpactuanteObj);


            /* IF SQLCODE NOT EQUAL ZEROS
                          INITIALIZE W-MENSAGEM-ERRO
                          STRING 'OPEN CURSOR CR_PACTUANTE.'
                                 ' CONTR TERC='   W-NUM-CONTRATO-TERC
                                 DELIMITED BY SIZE INTO W-MENSAGEM-ERRO
                          GO TO P9990-DB2-ERROR
                       END-IF */
            if (Sqlca.Sqlcode != 0)
            {
                WMensagemErro = Utility.PadLeft("", 500, " ");


                StringBuilder builder1 = new StringBuilder();
                builder1.Append("OPEN CURSOR CR_PACTUANTE.").Append(" CONTR TERC=").Append(SerializerUtils.Serialize(WNumContratoTerc, (DataTypeMetadata)DataTypeMap["WNumContratoTerc"]));
                string builderOut1 = builder1.ToString();
                if (builderOut1.Length > 500)
                {
                    builderOut1 = builderOut1.SafeSubstring(0, 500);
                }
                WMensagemErro = builderOut1;


                Szemb188P9990db2error();


            }


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188Db030abrirpactuante Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188Db040lerpactuante()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188Db040lerpactuante");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE 'DB040'                  TO W-LABEL */
            WLabel = Utility.PadStrToSize("DB040", 5);


            /* *>EXECSQL EXEC SQL
                  *>EXECSQL FETCH CR_PACTUANTE
                  *>EXECSQL INTO :SZ008-NOM-RAZ-SOCIAL
                  *>EXECSQL , :SZ008-NUM-CPF-CNPJ
                  *>EXECSQL , :SZ053-DTA-NASCIMENTO
                  *>EXECSQL , :SZ008-NUM-PESSOA
                  *>EXECSQL , :SZ053-IND-SEXO
                  *>EXECSQL , :SZ053-IND-ESTADO-CIVIL
                  *>EXECSQL , :SZ053-NUM-OCUPACAO
                  *>EXECSQL END-EXEC } */
            NamedQueryResponseSzemb188Crpactuante NamedQueryResponseSzemb188CrpactuanteResp = szemb188CursorCrPactuante.Fetch();
            if (!szemb188CursorCrPactuante.IsEOF())
            {
                DclszPessoa.Sz008NomRazSocial = NamedQueryResponseSzemb188CrpactuanteResp.NomRazSocial;
                DclszPessoa.Sz008NumCpfCnpj = Convert.ToInt64(NamedQueryResponseSzemb188CrpactuanteResp.NumCpfCnpj);
                DclszPessoaFisica.Sz053DtaNascimento = NamedQueryResponseSzemb188CrpactuanteResp.DtaNascimento.ToString(Utility.DEFAULT_DATE_FORMAT);
                DclszPessoa.Sz008NumPessoa = Convert.ToInt64(NamedQueryResponseSzemb188CrpactuanteResp.NumPessoa);
                DclszPessoaFisica.Sz053IndSexo = NamedQueryResponseSzemb188CrpactuanteResp.IndSexo;
                DclszPessoaFisica.Sz053IndEstadoCivil = NamedQueryResponseSzemb188CrpactuanteResp.IfnullSz053IndEstado8E4D7E;
                DclszPessoaFisica.Sz053NumOcupacao = Convert.ToInt64(NamedQueryResponseSzemb188CrpactuanteResp.Cbo);
            }


            /* EVALUATE SQLCODE
                       WHEN 000
                          ADD 1                      TO W-TOT-LIDOS-CERT-PACT
                          MOVE SZ008-NUM-PESSOA      TO W-COD-PESSOA
                       WHEN + 100
                          MOVE 'SIM'                 TO W-FIM-PACTUANTE
                  *>EXECSQL EXEC SQL CLOSE CR_PACTUANTE END-EXEC }
                       WHEN OTHER
                         INITIALIZE W-MENSAGEM-ERRO
                         STRING 'FETCH CURSOR CR_PACTUANTE.'
                                ' CONTR TERC='   W-NUM-CONTRATO-TERC
                                 DELIMITED BY SIZE INTO W-MENSAGEM-ERRO
                         GO TO P9990-DB2-ERROR
                       END-EVALUATE */
            if (Sqlca.Sqlcode == 000)
            {
                WTotLidosCertPact = Math.Abs(Utility.Truncate(WTotLidosCertPact + 1));


                WCodPessoa = Math.Abs(Convert.ToInt64(DclszPessoa.Sz008NumPessoa));


            }
            else if (Sqlca.Sqlcode == +100)
            {
                WFimPactuante = Utility.PadStrToSize("SIM", 3);


                szemb188CursorCrPactuante.Close();


            }
            else
            {
                WMensagemErro = Utility.PadLeft("", 500, " ");


                StringBuilder builder1 = new StringBuilder();
                builder1.Append("FETCH CURSOR CR_PACTUANTE.").Append(" CONTR TERC=").Append(SerializerUtils.Serialize(WNumContratoTerc, (DataTypeMetadata)DataTypeMap["WNumContratoTerc"]));
                string builderOut1 = builder1.ToString();
                if (builderOut1.Length > 500)
                {
                    builderOut1 = builderOut1.SafeSubstring(0, 500);
                }
                WMensagemErro = builderOut1;


                Szemb188P9990db2error();


            }


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188Db040lerpactuante Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188Db050lerendereco()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188Db050lerendereco");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE 'DB050'                  TO W-LABEL */
            WLabel = Utility.PadStrToSize("DB050", 5);


            /* *>EXECSQL EXEC  SQL
                  *>EXECSQL SELECT STRIP(NOM_LOGRADOURO)
                  *>EXECSQL , CASE WHEN NUM_LOGRADOURO IS NULL
                  *>EXECSQL OR NUM_LOGRADOURO = ''   THEN 'NF'
                  *>EXECSQL WHEN ASCII(SUBSTR(NUM_LOGRADOURO,1,1))
                  *>EXECSQL NOT BETWEEN 48 AND 57 THEN NUM_LOGRADOURO
                  *>EXECSQL ELSE LPAD(TRIM(NUM_LOGRADOURO),6,'0')
                  *>EXECSQL END AS NUM_END
                  *>EXECSQL , VALUE(DES_COMPL_ENDERECO, ' ')
                  *>EXECSQL , VALUE(NOM_BAIRRO, VALUE(DES_COMPL_ENDERECO, ' '))
                  *>EXECSQL , RTRIM(NOM_CIDADE) || '/' || VALUE(COD_UF, ' ')
                  *>EXECSQL , VALUE(COD_CEP,0)
                  *>EXECSQL INTO :SZ063-NOM-LOGRADOURO
                  *>EXECSQL , :SZ063-NUM-LOGRADOURO
                  *>EXECSQL , :SZ063-DES-COMPL-ENDERECO
                  *>EXECSQL , :SZ063-NOM-BAIRRO
                  *>EXECSQL , :SZ063-NOM-CIDADE
                  *>EXECSQL , :SZ063-COD-CEP
                  *>EXECSQL FROM dbo.SZ_ENDERECO A
                  *>EXECSQL WHERE NUM_ENDERECO IN
                  *>EXECSQL ( SELECT NUM_ENDERECO
                  *>EXECSQL FROM dbo.SZ_OBJ_ENDERECO
                  *>EXECSQL WHERE NUM_CONTRATO    = :SZ012-NUM-CONTRATO
                  *>EXECSQL AND STA_ENDERECO    = 'A'
                  *>EXECSQL AND COD_TP_ENDERECO IN('1','8','9')
                  *>EXECSQL )
                  *>EXECSQL FETCH FIRST 1 ROW ONLY
                  *>EXECSQL
                  *>EXECSQL END-EXEC } */
            // Create the input Object
            NamedQueryInputSzemb188Db050Lerendereco NamedQueryInputSzemb188Db050LerenderecoObj = new NamedQueryInputSzemb188Db050Lerendereco();
            NamedQueryInputSzemb188Db050LerenderecoObj.Sz012NumContrato = Convert.ToInt32(DclszContrTerc.Sz012NumContrato);

            // Execute the NamedQuery
            NHibernateUtilSzemb188 nhUtil = new NHibernateUtilSzemb188(Sqlca);
            List<NamedQueryResponseSzemb188Db050Lerendereco> NamedQueryResponseSzemb188Db050LerenderecoResp = nhUtil.ExecuteSelectNamedQuerySzemb188Db050Lerendereco(NamedQueryInputSzemb188Db050LerenderecoObj);
            if (NamedQueryResponseSzemb188Db050LerenderecoResp.Count > 0)
            {
                // Extract data from the output object
                for (int i = 0; i < NamedQueryResponseSzemb188Db050LerenderecoResp.Count; i++)
                {
                    NamedQueryResponseSzemb188Db050Lerendereco obj = NamedQueryResponseSzemb188Db050LerenderecoResp[i];
                    DclszEndereco.Sz063NomLogradouro = obj.StripNomLogradouro;
                    DclszEndereco.Sz063NumLogradouro = obj.NumEnd;
                    DclszEndereco.Sz063DesComplEndereco = obj.ValueDesComplEndereco;
                    DclszEndereco.Sz063NomBairro = obj.ValueNomBairroValue389Eb3D;
                    DclszEndereco.Sz063NomCidade = obj.RtrimNomCidadeValueCodUf;
                    DclszEndereco.Sz063CodCep = obj.ValueCodCep0;
                }
            }


            /* IF SQLCODE = 100
                          PERFORM DB051-LER-ENDERECO2
                       END-IF */
            if (Sqlca.Sqlcode == 100)
            {
                Szemb188Db051lerendereco2();


            }


            /* IF SQLCODE NOT EQUAL ZEROS AND 100
                          MOVE SZ012-NUM-CONTRATO    TO E-INTEGER-1
                          INITIALIZE W-MENSAGEM-ERRO
                          STRING 'SELECT SZ_OBJ_ENDERECO.'
                                 ' NUM_CONTRATO=' E-INTEGER-1
                                 ' CONTR TERC='   W-NUM-CONTRATO-TERC
                                 ' CONTR SEG='    W-NUM-CONTRATO
                                 ' NUM_PESSOA='   W-COD-PESSOA
                                DELIMITED BY SIZE INTO W-MENSAGEM-ERRO
                          GO TO P9990-DB2-ERROR
                       END-IF */
            if (Sqlca.Sqlcode != 0 && Sqlca.Sqlcode != 100)
            {
                EInteger1 = Math.Abs(Convert.ToInt64(DclszContrTerc.Sz012NumContrato));


                WMensagemErro = Utility.PadLeft("", 500, " ");


                StringBuilder builder1 = new StringBuilder();
                builder1.Append("SELECT SZ_OBJ_ENDERECO.").Append(" NUM_CONTRATO=").Append(SerializerUtils.Serialize(EInteger1, (DataTypeMetadata)DataTypeMap["EInteger1"])).Append(" CONTR TERC=").Append(SerializerUtils.Serialize(WNumContratoTerc, (DataTypeMetadata)DataTypeMap["WNumContratoTerc"])).Append(" CONTR SEG=").Append(SerializerUtils.Serialize(WNumContrato, (DataTypeMetadata)DataTypeMap["WNumContrato"])).Append(" NUM_PESSOA=").Append(SerializerUtils.Serialize(WCodPessoa, (DataTypeMetadata)DataTypeMap["WCodPessoa"]));
                string builderOut1 = builder1.ToString();
                if (builderOut1.Length > 500)
                {
                    builderOut1 = builderOut1.SafeSubstring(0, 500);
                }
                WMensagemErro = builderOut1;


                Szemb188P9990db2error();


            }


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188Db050lerendereco Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188Db051lerendereco2()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188Db051lerendereco2");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE 'DB051'                  TO W-LABEL */
            WLabel = Utility.PadStrToSize("DB051", 5);


            /* *>EXECSQL EXEC  SQL
                  *>EXECSQL SELECT STRIP(NOM_LOGRADOURO)
                  *>EXECSQL , CASE WHEN NUM_LOGRADOURO IS NULL
                  *>EXECSQL OR NUM_LOGRADOURO = ''   THEN 'NF'
                  *>EXECSQL WHEN ASCII(SUBSTR(NUM_LOGRADOURO,1,1))
                  *>EXECSQL NOT BETWEEN 48 AND 57 THEN NUM_LOGRADOURO
                  *>EXECSQL ELSE LPAD(TRIM(NUM_LOGRADOURO),6,'0')
                  *>EXECSQL END AS NUM_END
                  *>EXECSQL , VALUE(DES_COMPL_ENDERECO, ' ')
                  *>EXECSQL , VALUE(NOM_BAIRRO, VALUE(DES_COMPL_ENDERECO, ' '))
                  *>EXECSQL , RTRIM(NOM_CIDADE) || '/' || VALUE(COD_UF, ' ')
                  *>EXECSQL , VALUE(COD_CEP,0)
                  *>EXECSQL INTO :SZ063-NOM-LOGRADOURO
                  *>EXECSQL , :SZ063-NUM-LOGRADOURO
                  *>EXECSQL , :SZ063-DES-COMPL-ENDERECO
                  *>EXECSQL , :SZ063-NOM-BAIRRO
                  *>EXECSQL , :SZ063-NOM-CIDADE
                  *>EXECSQL , :SZ063-COD-CEP
                  *>EXECSQL FROM dbo.SZ_ENDERECO A
                  *>EXECSQL WHERE NUM_ENDERECO IN
                  *>EXECSQL ( SELECT NUM_ENDERECO
                  *>EXECSQL FROM dbo.SZ_OBJ_ENDERECO
                  *>EXECSQL WHERE NUM_CONTRATO    = :SZ012-NUM-CONTRATO
                  *>EXECSQL AND STA_ENDERECO    = 'A'
                  *>EXECSQL )
                  *>EXECSQL FETCH FIRST 1 ROW ONLY
                  *>EXECSQL
                  *>EXECSQL END-EXEC } */
            // Create the input Object
            NamedQueryInputSzemb188Db051Lerendereco2 NamedQueryInputSzemb188Db051Lerendereco2Obj = new NamedQueryInputSzemb188Db051Lerendereco2();
            NamedQueryInputSzemb188Db051Lerendereco2Obj.Sz012NumContrato = Convert.ToInt32(DclszContrTerc.Sz012NumContrato);

            // Execute the NamedQuery
            NHibernateUtilSzemb188 nhUtil = new NHibernateUtilSzemb188(Sqlca);
            List<NamedQueryResponseSzemb188Db051Lerendereco2> NamedQueryResponseSzemb188Db051Lerendereco2Resp = nhUtil.ExecuteSelectNamedQuerySzemb188Db051Lerendereco2(NamedQueryInputSzemb188Db051Lerendereco2Obj);
            if (NamedQueryResponseSzemb188Db051Lerendereco2Resp.Count > 0)
            {
                // Extract data from the output object
                for (int i = 0; i < NamedQueryResponseSzemb188Db051Lerendereco2Resp.Count; i++)
                {
                    NamedQueryResponseSzemb188Db051Lerendereco2 obj = NamedQueryResponseSzemb188Db051Lerendereco2Resp[i];
                    DclszEndereco.Sz063NomLogradouro = obj.StripNomLogradouro;
                    DclszEndereco.Sz063NumLogradouro = obj.NumEnd;
                    DclszEndereco.Sz063DesComplEndereco = obj.ValueDesComplEndereco;
                    DclszEndereco.Sz063NomBairro = obj.ValueNomBairroValue389Eb3D;
                    DclszEndereco.Sz063NomCidade = obj.RtrimNomCidadeValueCodUf;
                    DclszEndereco.Sz063CodCep = obj.ValueCodCep0;
                }
            }


            /* IF SQLCODE NOT EQUAL ZEROS
                          MOVE SZ012-NUM-CONTRATO    TO E-INTEGER-1
                          INITIALIZE W-MENSAGEM-ERRO
                          STRING 'SELECT SZ_OBJ_ENDERECO2.'
                                 ' NUM_CONTRATO=' E-INTEGER-1
                                 ' CONTR TERC='   W-NUM-CONTRATO-TERC
                                 ' CONTR SEG='    W-NUM-CONTRATO
                                 ' NUM_PESSOA='   W-COD-PESSOA
                                DELIMITED BY SIZE INTO W-MENSAGEM-ERRO
                          GO TO P9990-DB2-ERROR
                       END-IF */
            if (Sqlca.Sqlcode != 0)
            {
                EInteger1 = Math.Abs(Convert.ToInt64(DclszContrTerc.Sz012NumContrato));


                WMensagemErro = Utility.PadLeft("", 500, " ");


                StringBuilder builder1 = new StringBuilder();
                builder1.Append("SELECT SZ_OBJ_ENDERECO2.").Append(" NUM_CONTRATO=").Append(SerializerUtils.Serialize(EInteger1, (DataTypeMetadata)DataTypeMap["EInteger1"])).Append(" CONTR TERC=").Append(SerializerUtils.Serialize(WNumContratoTerc, (DataTypeMetadata)DataTypeMap["WNumContratoTerc"])).Append(" CONTR SEG=").Append(SerializerUtils.Serialize(WNumContrato, (DataTypeMetadata)DataTypeMap["WNumContrato"])).Append(" NUM_PESSOA=").Append(SerializerUtils.Serialize(WCodPessoa, (DataTypeMetadata)DataTypeMap["WCodPessoa"]));
                string builderOut1 = builder1.ToString();
                if (builderOut1.Length > 500)
                {
                    builderOut1 = builderOut1.SafeSubstring(0, 500);
                }
                WMensagemErro = builderOut1;


                Szemb188P9990db2error();


            }


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188Db051lerendereco2 Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188Db060acessavigenciacontr()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188Db060acessavigenciacontr");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE 'DB060'                  TO W-LABEL */
            WLabel = Utility.PadStrToSize("DB060", 5);


            /* INITIALIZE H-DTA-INI-VIGENCIA-ORIG
                                  H-DTA-FIM-CARENCIA-ORIG */
            HDtaIniVigenciaOrig = Utility.PadLeft("", 10, " ");
            HDtaFimCarenciaOrig = Utility.PadLeft("", 10, " ");


            /* *>EXECSQL EXEC SQL
                  *>EXECSQL SELECT SZ012.DTA_ASSINATURA
                  *>EXECSQL + :SZ021-QTD-MESES-CONTRATO months
                  *>EXECSQL AS DTA_FIM_CARENCIA
                  *>EXECSQL , SZ012.DTA_FIM_VIG_TERC     AS DTA_FIM_VIGENCIA
                  *>EXECSQL INTO :H-DTA-INI-VIGENCIA-ORIG
                  *>EXECSQL , :H-DTA-FIM-CARENCIA-ORIG
                  *>EXECSQL FROM dbo.SZ_CONTR_TERC SZ012
                  *>EXECSQL WHERE NUM_PES_OPERADOR          = :SZ012-NUM-PES-OPERADOR
                  *>EXECSQL AND NUM_LINHA_PRODUTO         = :SZ012-NUM-LINHA-PRODUTO
                  *>EXECSQL AND NUM_CONTRATO_TERC         = :SZ012-NUM-CONTRATO-ANT
                  *>EXECSQL
                  *>EXECSQL END-EXEC } */
            // Create the input Object
            NamedQueryInputSzemb188Db060Acessavigenciacontr NamedQueryInputSzemb188Db060AcessavigenciacontrObj = new NamedQueryInputSzemb188Db060Acessavigenciacontr();
            NamedQueryInputSzemb188Db060AcessavigenciacontrObj.Sz021QtdMesesContrato = DclszContrSeguro.Sz021QtdMesesContrato.ToString();
            NamedQueryInputSzemb188Db060AcessavigenciacontrObj.Sz012NumPesOperador = Convert.ToInt32(DclszContrTerc.Sz012NumPesOperador);
            NamedQueryInputSzemb188Db060AcessavigenciacontrObj.Sz012NumLinhaProduto = Convert.ToInt32(DclszContrTerc.Sz012NumLinhaProduto);
            NamedQueryInputSzemb188Db060AcessavigenciacontrObj.Sz012NumContratoAnt = Convert.ToInt64(DclszContrTerc.Sz012NumContratoAnt);

            // Execute the NamedQuery
            NHibernateUtilSzemb188 nhUtil = new NHibernateUtilSzemb188(Sqlca);
            List<NamedQueryResponseSzemb188Db060Acessavigenciacontr> NamedQueryResponseSzemb188Db060AcessavigenciacontrResp = nhUtil.ExecuteSelectNamedQuerySzemb188Db060Acessavigenciacontr(NamedQueryInputSzemb188Db060AcessavigenciacontrObj);
            if (NamedQueryResponseSzemb188Db060AcessavigenciacontrResp.Count > 0)
            {
                // Extract data from the output object
                for (int i = 0; i < NamedQueryResponseSzemb188Db060AcessavigenciacontrResp.Count; i++)
                {
                    NamedQueryResponseSzemb188Db060Acessavigenciacontr obj = NamedQueryResponseSzemb188Db060AcessavigenciacontrResp[i];
                    HDtaIniVigenciaOrig = obj.DtaFimCarencia;
                    HDtaFimCarenciaOrig = obj.DtaFimVigencia;
                }
            }


            /* IF SZEMB188-TRACE-ON-88
                          MOVE SQLCODE               TO E-SQLCODE
                          DISPLAY 'DB060-ACESSA-VIGENCIA-CONTR:'
                          ' SQLCODE='                    E-SQLCODE
                          ' SZ012-NUM-PES-OPERADOR='     SZ012-NUM-PES-OPERADOR
                          ' SZ012-NUM-LINHA-PRODUTO='    SZ012-NUM-LINHA-PRODUTO
                          ' SZ012-NUM-CONTRATO-ANT='     SZ012-NUM-CONTRATO-ANT
                          ' H-DTA-INI-VIGENCIA-ORIG='    H-DTA-INI-VIGENCIA-ORIG
                          ' H-DTA-FIM-CARENCIA-ORIG='    H-DTA-FIM-CARENCIA-ORIG
                          ' TIME=' FUNCTION CURRENT-DATE
                       END-IF */
            if (Szemb188Parametros.Szemb188Szemb188traceon88())
            {
                ESqlcode = Convert.ToInt64(Utility.PadNumToSize(Sqlca.Sqlcode, 4));


                Console.WriteLine("DB060-ACESSA-VIGENCIA-CONTR:" + " SQLCODE=" + ESqlcode + " SZ012-NUM-PES-OPERADOR=" + DclszContrTerc.Sz012NumPesOperador + " SZ012-NUM-LINHA-PRODUTO=" + DclszContrTerc.Sz012NumLinhaProduto + " SZ012-NUM-CONTRATO-ANT=" + DclszContrTerc.Sz012NumContratoAnt + " H-DTA-INI-VIGENCIA-ORIG=" + HDtaIniVigenciaOrig + " H-DTA-FIM-CARENCIA-ORIG=" + HDtaFimCarenciaOrig + " TIME=" + Utility.GetCurrentDate());


            }


            /* IF SQLCODE NOT = 000
                          MOVE SZ012-NUM-PES-OPERADOR
                                                     TO E-INTEGER-1
                          MOVE SZ012-NUM-LINHA-PRODUTO
                                                     TO E-SMALLINT-1
                          MOVE SZ012-NUM-CONTRATO-ANT
                                                     TO E-BIGINT-1
                          INITIALIZE W-MENSAGEM-ERRO
                          STRING 'SELECT SZ_CONTR_TERC.'
                                 ' PES-OPERADOR='  E-INTEGER-1
                                 ' LINHA-PRODUTO=' E-SMALLINT-1
                                 ' CONTRATO-ANT='  E-BIGINT-1
                                DELIMITED BY SIZE INTO W-MENSAGEM-ERRO
                          GO TO P9990-DB2-ERROR
                       END-IF */
            if (Sqlca.Sqlcode != 000)
            {
                EInteger1 = Math.Abs(Convert.ToInt64(DclszContrTerc.Sz012NumPesOperador));


                ESmallint1 = Math.Abs(Convert.ToInt64(DclszContrTerc.Sz012NumLinhaProduto));


                EBigint1 = Math.Abs(Convert.ToInt64(DclszContrTerc.Sz012NumContratoAnt));


                WMensagemErro = Utility.PadLeft("", 500, " ");


                StringBuilder builder1 = new StringBuilder();
                builder1.Append("SELECT SZ_CONTR_TERC.").Append(" PES-OPERADOR=").Append(SerializerUtils.Serialize(EInteger1, (DataTypeMetadata)DataTypeMap["EInteger1"])).Append(" LINHA-PRODUTO=").Append(SerializerUtils.Serialize(ESmallint1, (DataTypeMetadata)DataTypeMap["ESmallint1"])).Append(" CONTRATO-ANT=").Append(SerializerUtils.Serialize(EBigint1, (DataTypeMetadata)DataTypeMap["EBigint1"]));
                string builderOut1 = builder1.ToString();
                if (builderOut1.Length > 500)
                {
                    builderOut1 = builderOut1.SafeSubstring(0, 500);
                }
                WMensagemErro = builderOut1;


                Szemb188P9990db2error();


            }


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188Db060acessavigenciacontr Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188Db070lertelefone()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188Db070lertelefone");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE 'DB070'                  TO W-LABEL */
            WLabel = Utility.PadStrToSize("DB070", 5);


            /* MOVE 55                       TO  W-DDI */
            WDdi = Convert.ToInt64(55);


            /* *>EXECSQL EXEC SQL
                  *>EXECSQL SELECT SZ057.NUM_DDD
                  *>EXECSQL , SZ057.NUM_TELEFONE
                  *>EXECSQL INTO :SZ057-NUM-DDD
                  *>EXECSQL , :SZ057-NUM-TELEFONE
                  *>EXECSQL FROM dbo.SZ_PESSOA_TELEFONE SZ057
                  *>EXECSQL WHERE NUM_PESSOA = :SZ008-NUM-PESSOA
                  *>EXECSQL AND SZ057.SEQ_TELEFONE =
                  *>EXECSQL ( SELECT MAX(SZ0571.SEQ_TELEFONE)
                  *>EXECSQL FROM dbo.SZ_PESSOA_TELEFONE SZ0571
                  *>EXECSQL WHERE SZ0571.NUM_PESSOA = SZ057.NUM_PESSOA
                  *>EXECSQL )
                  *>EXECSQL
                  *>EXECSQL END-EXEC } */
            // Create the input Object
            NamedQueryInputSzemb188Db070Lertelefone NamedQueryInputSzemb188Db070LertelefoneObj = new NamedQueryInputSzemb188Db070Lertelefone();
            NamedQueryInputSzemb188Db070LertelefoneObj.Sz008NumPessoa = Convert.ToInt32(DclszPessoa.Sz008NumPessoa);

            // Execute the NamedQuery
            NHibernateUtilSzemb188 nhUtil = new NHibernateUtilSzemb188(Sqlca);
            List<NamedQueryResponseSzemb188Db070Lertelefone> NamedQueryResponseSzemb188Db070LertelefoneResp = nhUtil.ExecuteSelectNamedQuerySzemb188Db070Lertelefone(NamedQueryInputSzemb188Db070LertelefoneObj);
            if (NamedQueryResponseSzemb188Db070LertelefoneResp.Count > 0)
            {
                // Extract data from the output object
                for (int i = 0; i < NamedQueryResponseSzemb188Db070LertelefoneResp.Count; i++)
                {
                    NamedQueryResponseSzemb188Db070Lertelefone obj = NamedQueryResponseSzemb188Db070LertelefoneResp[i];
                    DclszPessoaTelefone.Sz057NumDdd = Convert.ToInt64(obj.NumDdd);
                    DclszPessoaTelefone.Sz057NumTelefone = Convert.ToInt64(obj.NumTelefone);
                }
            }


            /* IF SQLCODE  EQUAL 100
                          MOVE ZEROS                 TO  W-DDI
                                         SZ057-NUM-DDD
                                         SZ057-NUM-TELEFONE
                       ELSE
                         IF SQLCODE NOT EQUAL ZEROS
                            INITIALIZE W-MENSAGEM-ERRO
                            STRING 'SELECT SZ_PESSOA_TELEFONE.'
                                   ' CONTR TERC='   W-NUM-CONTRATO-TERC
                                   ' CONTR SEG='    W-NUM-CONTRATO
                                   ' NUM_PESSOA='   W-COD-PESSOA
                                  DELIMITED BY SIZE INTO W-MENSAGEM-ERRO
                            GO TO P9990-DB2-ERROR
                         END-IF
                       END-IF */
            if (Sqlca.Sqlcode == 100)
            {
                WDdi = Convert.ToInt64(0);
                DclszPessoaTelefone.Sz057NumDdd = Convert.ToInt64(0);
                DclszPessoaTelefone.Sz057NumTelefone = Convert.ToInt64(0);


            }
            else
            {
                if (Sqlca.Sqlcode != 0)
                {
                    WMensagemErro = Utility.PadLeft("", 500, " ");


                    StringBuilder builder1 = new StringBuilder();
                    builder1.Append("SELECT SZ_PESSOA_TELEFONE.").Append(" CONTR TERC=").Append(SerializerUtils.Serialize(WNumContratoTerc, (DataTypeMetadata)DataTypeMap["WNumContratoTerc"])).Append(" CONTR SEG=").Append(SerializerUtils.Serialize(WNumContrato, (DataTypeMetadata)DataTypeMap["WNumContrato"])).Append(" NUM_PESSOA=").Append(SerializerUtils.Serialize(WCodPessoa, (DataTypeMetadata)DataTypeMap["WCodPessoa"]));
                    string builderOut1 = builder1.ToString();
                    if (builderOut1.Length > 500)
                    {
                        builderOut1 = builderOut1.SafeSubstring(0, 500);
                    }
                    WMensagemErro = builderOut1;


                    Szemb188P9990db2error();


                }



            }


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188Db070lertelefone Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188Db080descprofissao()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188Db080descprofissao");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE 'DB080'                  TO W-LABEL */
            WLabel = Utility.PadStrToSize("DB080", 5);


            /* *>EXECSQL EXEC  SQL
                  *>EXECSQL SELECT DESCR_CBO
                  *>EXECSQL INTO :CBO-DESCR-CBO
                  *>EXECSQL FROM  dbo.CBO
                  *>EXECSQL WHERE COD_CBO     = :SZ053-NUM-OCUPACAO
                  *>EXECSQL
                  *>EXECSQL END-EXEC } */
            // Create the input Object
            NamedQueryInputSzemb188Db080Descprofissao NamedQueryInputSzemb188Db080DescprofissaoObj = new NamedQueryInputSzemb188Db080Descprofissao();
            NamedQueryInputSzemb188Db080DescprofissaoObj.Sz053NumOcupacao = Convert.ToInt32(DclszPessoaFisica.Sz053NumOcupacao);

            // Execute the NamedQuery
            NHibernateUtilSzemb188 nhUtil = new NHibernateUtilSzemb188(Sqlca);
            List<NamedQueryResponseSzemb188Db080Descprofissao> NamedQueryResponseSzemb188Db080DescprofissaoResp = nhUtil.ExecuteSelectNamedQuerySzemb188Db080Descprofissao(NamedQueryInputSzemb188Db080DescprofissaoObj);
            if (NamedQueryResponseSzemb188Db080DescprofissaoResp.Count > 0)
            {
                // Extract data from the output object
                for (int i = 0; i < NamedQueryResponseSzemb188Db080DescprofissaoResp.Count; i++)
                {
                    NamedQueryResponseSzemb188Db080Descprofissao obj = NamedQueryResponseSzemb188Db080DescprofissaoResp[i];
                    Dclcbo.CboDescrCbo = obj.DescrCbo;
                }
            }


            /* IF SQLCODE  EQUAL 100
                          MOVE  SPACES               TO CBO-DESCR-CBO
                       ELSE
                        IF SQLCODE NOT EQUAL ZEROS
                           MOVE SZ053-NUM-OCUPACAO   TO W-NUM-OCUPACAO
                           MOVE SZ072-COD-ACOPLADO   TO E-INTEGER-1
                           MOVE SZ011-COD-PRODUTO    TO E-INTEGER-2
                           INITIALIZE W-MENSAGEM-ERRO
                           STRING 'SELECT dbo.CBO. '
                                  ' COD_CBO='      W-NUM-OCUPACAO
                                  ' COD-ACOPLADO=' E-INTEGER-1
                                  ' COD-PRODUTO= ' E-INTEGER-2
                                 DELIMITED BY SIZE INTO W-MENSAGEM-ERRO
                          GO TO P9990-DB2-ERROR
                        END-IF
                       END-IF */
            if (Sqlca.Sqlcode == 100)
            {
                Dclcbo.CboDescrCbo = Utility.PadStrToSize(" ", 50);


            }
            else
            {
                if (Sqlca.Sqlcode != 0)
                {
                    WNumOcupacao = Math.Abs(Convert.ToInt64(DclszPessoaFisica.Sz053NumOcupacao));


                    EInteger1 = Math.Abs(Convert.ToInt64(DclszAcoplado.Sz072CodAcoplado));


                    EInteger2 = Math.Abs(Convert.ToInt64(DclszOrigemContrato.Sz011CodProduto));


                    WMensagemErro = Utility.PadLeft("", 500, " ");


                    StringBuilder builder1 = new StringBuilder();
                    builder1.Append("SELECT dbo.CBO. ").Append(" COD_CBO=").Append(SerializerUtils.Serialize(WNumOcupacao, (DataTypeMetadata)DataTypeMap["WNumOcupacao"])).Append(" COD-ACOPLADO=").Append(SerializerUtils.Serialize(EInteger1, (DataTypeMetadata)DataTypeMap["EInteger1"])).Append(" COD-PRODUTO= ").Append(SerializerUtils.Serialize(EInteger2, (DataTypeMetadata)DataTypeMap["EInteger2"]));
                    string builderOut1 = builder1.ToString();
                    if (builderOut1.Length > 500)
                    {
                        builderOut1 = builderOut1.SafeSubstring(0, 500);
                    }
                    WMensagemErro = builderOut1;


                    Szemb188P9990db2error();


                }



            }


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188Db080descprofissao Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188P9000finaliza()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188P9000finaliza");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE 'P9000'                  TO W-LABEL */
            WLabel = Utility.PadStrToSize("P9000", 5);


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188P9000finaliza Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188Db900executacommit()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188Db900executacommit");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* *>EXECSQL EXEC SQL COMMIT END-EXEC } */
            // COMMIT


            /* IF SZEMB188-TRACE-ON-88
                          DISPLAY W-PROGRAMA '- EXECUTOU COMMIT - '
                                  FUNCTION CURRENT-DATE
                       END-IF */
            if (Szemb188Parametros.Szemb188Szemb188traceon88())
            {
                Console.WriteLine(WPrograma + "- EXECUTOU COMMIT - " + Utility.GetCurrentDate());


            }


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188Db900executacommit Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188Db905executarollback()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188Db905executarollback");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* *>EXECSQL EXEC SQL ROLLBACK END-EXEC } */
            // ROLLBACK


            /* DISPLAY ' ' */
            Console.WriteLine(" ");


            /* DISPLAY '$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$'
                               '$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$' */
            Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$" + "$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");


            /* DISPLAY W-PROGRAMA
                               '- EXECUTOU ROLLBACK' */
            Console.WriteLine(WPrograma + "- EXECUTOU ROLLBACK");


            /* DISPLAY '$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$'
                               '$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$' */
            Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$" + "$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188Db905executarollback Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188P9990db2error()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188P9990db2error");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE  99                      TO  W-RETURN-CODE */
            WReturnCode = Math.Abs(Convert.ToInt64(99));


            /* DISPLAY ' ' */
            Console.WriteLine(" ");


            /* DISPLAY '################################################'
                               '################################################' */
            Console.WriteLine("################################################" + "################################################");


            /* DISPLAY W-PROGRAMA ' - ERRO DE ACESSO AO BANCO DE DADOS DB2' */
            Console.WriteLine(WPrograma + " - ERRO DE ACESSO AO BANCO DE DADOS DB2");


            /* DISPLAY '################################################'
                               '################################################' */
            Console.WriteLine("################################################" + "################################################");


            /* DISPLAY 'PARAGRAFO DE ORIGEM=' W-LABEL */
            Console.WriteLine("PARAGRAFO DE ORIGEM=" + WLabel);


            /* MOVE SQLCODE                  TO E-SQLCODE */
            ESqlcode = Convert.ToInt64(Utility.PadNumToSize(Sqlca.Sqlcode, 4));


            /* DISPLAY 'SQLCODE            =' E-SQLCODE */
            Console.WriteLine("SQLCODE            =" + ESqlcode);


            /* MOVE SQLERRD(3)               TO E-SQLCODE */
            ESqlcode = Utility.PadNumToSize(Sqlca.Sqlerrd[2], 4);


            /* DISPLAY 'SQLERRD(3)         =' E-SQLCODE */
            Console.WriteLine("SQLERRD(3)         =" + ESqlcode);


            /* IF SQLERRMC NOT = SPACES
                          DISPLAY 'SQLERRMC=<'   SQLERRMC '>'
                       END-IF */
            if (Utility.CompareWithSpaces(Sqlca.Sqlerrm.Sqlerrmc) != 0)
            {
                Console.WriteLine("SQLERRMC=<" + Sqlca.Sqlerrm.Sqlerrmc + ">");


            }


            /* DISPLAY '------------------------------------------------'
                               '------------------------------------------------' */
            Console.WriteLine("------------------------------------------------" + "------------------------------------------------");


            /* DISPLAY 'MENSAGEM:' */
            Console.WriteLine("MENSAGEM:");


            /* DISPLAY '------------------------------------------------'
                         '------------------------------------------------------' */
            Console.WriteLine("------------------------------------------------" + "------------------------------------------------------");


            /* DISPLAY '<' W-MENSAGEM-ERRO(001:100) '>' */
            Console.WriteLine("<" + Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 0, 100) + ">");


            /* IF W-MENSAGEM-ERRO(101:100) NOT = SPACES
                          DISPLAY '<' W-MENSAGEM-ERRO(101:100) '>'
                       END-IF */
            if (Utility.CompareWithSpaces(Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 100, 100)) != 0)
            {
                Console.WriteLine("<" + Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 100, 100) + ">");


            }


            /* IF W-MENSAGEM-ERRO(201:100) NOT = SPACES
                          DISPLAY '<' W-MENSAGEM-ERRO(201:100) '>'
                       END-IF */
            if (Utility.CompareWithSpaces(Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 200, 100)) != 0)
            {
                Console.WriteLine("<" + Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 200, 100) + ">");


            }


            /* IF W-MENSAGEM-ERRO(301:100) NOT = SPACES
                          DISPLAY '<' W-MENSAGEM-ERRO(301:100) '>'
                       END-IF */
            if (Utility.CompareWithSpaces(Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 300, 100)) != 0)
            {
                Console.WriteLine("<" + Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 300, 100) + ">");


            }


            /* IF W-MENSAGEM-ERRO(401:100) NOT = SPACES
                          DISPLAY '<' W-MENSAGEM-ERRO(401:100) '>'
                       END-IF */
            if (Utility.CompareWithSpaces(Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 400, 100)) != 0)
            {
                Console.WriteLine("<" + Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 400, 100) + ">");


            }


            /* DISPLAY '################################################'
                               '################################################' */
            Console.WriteLine("################################################" + "################################################");


            /* PERFORM DB905-EXECUTA-ROLLBACK */
            Szemb188Db905executarollback();


            /* MOVE 2                        TO H-SZL01-IND-ERRO-LOG */
            HSzl01Szemnl01.HSzl01IndErroLog = Convert.ToInt64(2);


            /* MOVE 0                        TO N-SZL01-IND-ERRO-LOG */
            NSzl01Szemnl01.NSzl01IndErroLog = Convert.ToInt64(0);


            /* PERFORM C0010-CALL-SP-SZEMNL01 */
            Szemb188C0010callspszemnl01();


            /* GO TO P9999-FINALIZACAO */
            Szemb188P9999finalizacao();


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188P9990db2error Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188P9994fimanormal()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188P9994fimanormal");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE  99                      TO  W-RETURN-CODE */
            WReturnCode = Math.Abs(Convert.ToInt64(99));


            /* DISPLAY ' ' */
            Console.WriteLine(" ");


            /* DISPLAY '################################################'
                               '################################################' */
            Console.WriteLine("################################################" + "################################################");


            /* DISPLAY W-PROGRAMA ' - PROCESSAMENTO COM ERRO' */
            Console.WriteLine(WPrograma + " - PROCESSAMENTO COM ERRO");


            /* DISPLAY '################################################'
                               '################################################' */
            Console.WriteLine("################################################" + "################################################");


            /* DISPLAY 'PARAGRAFO DE ORIGEM=' W-LABEL */
            Console.WriteLine("PARAGRAFO DE ORIGEM=" + WLabel);


            /* DISPLAY '------------------------------------------------'
                               '------------------------------------------------' */
            Console.WriteLine("------------------------------------------------" + "------------------------------------------------");


            /* DISPLAY 'MENSAGEM:' */
            Console.WriteLine("MENSAGEM:");


            /* DISPLAY '------------------------------------------------'
                         '------------------------------------------------------' */
            Console.WriteLine("------------------------------------------------" + "------------------------------------------------------");


            /* DISPLAY '<' W-MENSAGEM-ERRO(001:100) '>' */
            Console.WriteLine("<" + Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 0, 100) + ">");


            /* IF W-MENSAGEM-ERRO(101:100) NOT = SPACES
                          DISPLAY '<' W-MENSAGEM-ERRO(101:100) '>'
                       END-IF */
            if (Utility.CompareWithSpaces(Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 100, 100)) != 0)
            {
                Console.WriteLine("<" + Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 100, 100) + ">");


            }


            /* IF W-MENSAGEM-ERRO(201:100) NOT = SPACES
                          DISPLAY '<' W-MENSAGEM-ERRO(201:100) '>'
                       END-IF */
            if (Utility.CompareWithSpaces(Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 200, 100)) != 0)
            {
                Console.WriteLine("<" + Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 200, 100) + ">");


            }


            /* IF W-MENSAGEM-ERRO(301:100) NOT = SPACES
                          DISPLAY '<' W-MENSAGEM-ERRO(301:100) '>'
                       END-IF */
            if (Utility.CompareWithSpaces(Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 300, 100)) != 0)
            {
                Console.WriteLine("<" + Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 300, 100) + ">");


            }


            /* IF W-MENSAGEM-ERRO(401:100) NOT = SPACES
                          DISPLAY '<' W-MENSAGEM-ERRO(401:100) '>'
                       END-IF */
            if (Utility.CompareWithSpaces(Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 400, 100)) != 0)
            {
                Console.WriteLine("<" + Utility.RefMod(WMensagemErro, (DataTypeMetadata)DataTypeMap["WMensagemErro"], 400, 100) + ">");


            }


            /* DISPLAY '################################################'
                               '################################################' */
            Console.WriteLine("################################################" + "################################################");


            /* PERFORM DB905-EXECUTA-ROLLBACK */
            Szemb188Db905executarollback();


            /* MOVE 1                        TO H-SZL01-IND-ERRO-LOG */
            HSzl01Szemnl01.HSzl01IndErroLog = Convert.ToInt64(1);


            /* MOVE 0                        TO N-SZL01-IND-ERRO-LOG */
            NSzl01Szemnl01.NSzl01IndErroLog = Convert.ToInt64(0);


            /* PERFORM C0010-CALL-SP-SZEMNL01 */
            Szemb188C0010callspszemnl01();


            /* GO TO P9999-FINALIZACAO */
            Szemb188P9999finalizacao();


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188P9994fimanormal Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188P9999finalizacao()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188P9999finalizacao");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* IF W-OPEN-ARQ = 'SIM'
                          MOVE 'NAO'                 TO W-OPEN-ARQ
                          CLOSE ARQTEMP1
                       END-IF */
            if ((Comparisions.SafeStringCompare(WOpenArq, "SIM")))
            {
                WOpenArq = Utility.PadStrToSize("NAO", 3);


                Arqtemp1.Close();


            }


            /* DISPLAY ' ' */
            Console.WriteLine(" ");


            /* DISPLAY '---------------------------------------------------'
                               '---------------------------------------------------' */
            Console.WriteLine("---------------------------------------------------" + "---------------------------------------------------");


            /* DISPLAY W-PROGRAMA ' - TOTAIS DO PROCESSAMENTO' */
            Console.WriteLine(WPrograma + " - TOTAIS DO PROCESSAMENTO");


            /* DISPLAY '---------------------------------------------------'
                               '---------------------------------------------------' */
            Console.WriteLine("---------------------------------------------------" + "---------------------------------------------------");


            /* DISPLAY 'QTD CERTIFICADOS LIDOS COMPRA   = '
                                                               W-TOT-LIDOS-CERT-COMP */
            Console.WriteLine("QTD CERTIFICADOS LIDOS COMPRA   = " + WTotLidosCertComp);


            /* DISPLAY 'QTD CERTIFICADOS LIDOS PACTUANTE= '
                                                               W-TOT-LIDOS-CERT-PACT */
            Console.WriteLine("QTD CERTIFICADOS LIDOS PACTUANTE= " + WTotLidosCertPact);


            /* DISPLAY 'QTD COMPRA SAF/CESTA            = ' W-QTD-COMPRA1 */
            Console.WriteLine("QTD COMPRA SAF/CESTA            = " + WQtdCompra1);


            /* DISPLAY 'QTD RE-COMPRA SAF/CESTA         = ' W-QTD-COMPRA2 */
            Console.WriteLine("QTD RE-COMPRA SAF/CESTA         = " + WQtdCompra2);


            /* DISPLAY 'QTD INCLUIDO OBJ_ACOPLADO       = ' W-ATUAL-OBJ-ACOP */
            Console.WriteLine("QTD INCLUIDO OBJ_ACOPLADO       = " + WAtualObjAcop);


            /* DISPLAY 'QTD INCLUIDO OBJ_ACOPL_ASSIST   = '
                                                               W-ATUAL-OBJ-ACOP-ASS */
            Console.WriteLine("QTD INCLUIDO OBJ_ACOPL_ASSIST   = " + WAtualObjAcopAss);


            /* DISPLAY 'QTD REGISTROS GRAVADOS ARQUIVO  = ' W-TOT-GRAVADOS */
            Console.WriteLine("QTD REGISTROS GRAVADOS ARQUIVO  = " + WTotGravados);


            /* DISPLAY '---------------------------------------------------'
                               '---------------------------------------------------' */
            Console.WriteLine("---------------------------------------------------" + "---------------------------------------------------");


            /* DISPLAY ' ' */
            Console.WriteLine(" ");


            /* IF W-RETURN-CODE = 0
                          DISPLAY '************************************************'
                                  '************************************************'
                          DISPLAY W-PROGRAMA '- FIM DE PROCESSAMENTO OK EM  '
                                 FUNCTION CURRENT-DATE(07:2) '/'
                                 FUNCTION CURRENT-DATE(05:2) '/'
                                 FUNCTION CURRENT-DATE(01:4) ' AS '
                                 FUNCTION CURRENT-DATE(09:2) ':'
                                 FUNCTION CURRENT-DATE(11:2) ':'
                                 FUNCTION CURRENT-DATE(13:2)
                          DISPLAY '************************************************'
                                  '************************************************'
                       ELSE
                          DISPLAY '################################################'
                                  '################################################'
                          DISPLAY W-PROGRAMA '- FIM DE PROCESSAMENTO COM ERRO EM '
                                 FUNCTION CURRENT-DATE(07:2) '/'
                                 FUNCTION CURRENT-DATE(05:2) '/'
                                 FUNCTION CURRENT-DATE(01:4) ' AS '
                                 FUNCTION CURRENT-DATE(09:2) ':'
                                 FUNCTION CURRENT-DATE(11:2) ':'
                                 FUNCTION CURRENT-DATE(13:2)
                          DISPLAY '################################################'
                                  '################################################'
                       END-IF */
            if (WReturnCode == 0)
            {
                Console.WriteLine("************************************************" + "************************************************");


                Console.WriteLine(WPrograma + "- FIM DE PROCESSAMENTO OK EM  " + Utility.GetCurrentDate().SafeSubstring((int)07 - 1, 2) + "/" + Utility.GetCurrentDate().SafeSubstring((int)05 - 1, 2) + "/" + Utility.GetCurrentDate().SafeSubstring((int)01 - 1, 4) + " AS " + Utility.GetCurrentDate().SafeSubstring((int)09 - 1, 2) + ":" + Utility.GetCurrentDate().SafeSubstring((int)11 - 1, 2) + ":" + Utility.GetCurrentDate().SafeSubstring((int)13 - 1, 2));


                Console.WriteLine("************************************************" + "************************************************");


            }
            else
            {
                Console.WriteLine("################################################" + "################################################");



                Console.WriteLine(WPrograma + "- FIM DE PROCESSAMENTO COM ERRO EM " + Utility.GetCurrentDate().SafeSubstring((int)07 - 1, 2) + "/" + Utility.GetCurrentDate().SafeSubstring((int)05 - 1, 2) + "/" + Utility.GetCurrentDate().SafeSubstring((int)01 - 1, 4) + " AS " + Utility.GetCurrentDate().SafeSubstring((int)09 - 1, 2) + ":" + Utility.GetCurrentDate().SafeSubstring((int)11 - 1, 2) + ":" + Utility.GetCurrentDate().SafeSubstring((int)13 - 1, 2));



                Console.WriteLine("################################################" + "################################################");



            }


            /* DISPLAY ' ' */
            Console.WriteLine(" ");


            /* PERFORM PMONITOR-ATUALIZA-MONITOR */
            Szemb188Pmonitoratualizamonitor();


            /* MOVE W-RETURN-CODE(2:5)       TO LK-GE3000B-COD-PROCESSAMENTO */
            LkGe3000BParametros.LkGe3000BCodProcessamento = Utility.PadStrToSize(Utility.RefMod(WReturnCode, (DataTypeMetadata)DataTypeMap["WReturnCode"], 1, 5), 5);


            /* MOVE H-SZL01-SEQ-LOG-SISTEMA  TO LK-GE3000B-SEQ-LOG-SISTEMA */
            LkGe3000BParametros.LkGe3000BSeqLogSistema = Math.Abs(Convert.ToInt64(HSzl01Szemnl01.HSzl01SeqLogSistema));


            /* PERFORM PMONITOR-FINALIZACAO */
            Szemb188Pmonitorfinalizacao();


            /* PERFORM DB900-EXECUTA-COMMIT */
            Szemb188Db900executacommit();


            /* MOVE W-RETURN-CODE            TO RETURN-CODE */
            RETURN_CODE = Convert.ToInt64(WReturnCode);


            /* STOP RUN */
           //HEBERT
            throw new ExitException();

            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188P9999finalizacao Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188Pmonitorinicializacao()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188Pmonitorinicializacao");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE 'INI'                    TO LK-GE3000B-OPERACAO */
            LkGe3000BParametros.LkGe3000BOperacao = Utility.PadStrToSize("INI", 3);


            /* PERFORM PMONITOR-CALL-GE3000B THRU PMONITOR-CALL-GE3000B-EXIT */
            Szemb188Pmonitorcallge3000b(); Szemb188Pmonitorcallge3000bexit();


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188Pmonitorinicializacao Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188Pmonitorgravamonitor()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188Pmonitorgravamonitor");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE 'PMGMO'                  TO W-LABEL
                                                        W-GE3000B-PARAGRAFO-ORIG */
            WLabel = Utility.PadStrToSize("PMGMO", 5);
            WGe3000BParagrafoOrig = Utility.PadStrToSize("PMGMO", 5);


            /* MOVE 'GRA'                    TO LK-GE3000B-OPERACAO */
            LkGe3000BParametros.LkGe3000BOperacao = Utility.PadStrToSize("GRA", 3);


            /* MOVE 'I'                      TO LK-GE3000B-PROC-ARQ */
            LkGe3000BParametros.LkGe3000BProcArq = Utility.PadStrToSize("I", 1);


            /* INITIALIZE LK-GE3000B-NUM-VERSAO
                                  LK-GE3000B-NUM-PES-OPERADOR
                                  LK-GE3000B-NUM-LINHA-PRODUTO
                                  LK-GE3000B-NUM-CONTRATO-TERC
                                  LK-GE3000B-NUM-CONTRATO
                                  LK-GE3000B-NUM-ITEM-MOV */
            LkGe3000BParametros.LkGe3000BNumVersao = 0;
            LkGe3000BParametros.LkGe3000BNumPesOperador = 0;
            LkGe3000BParametros.LkGe3000BNumLinhaProduto = 0;
            LkGe3000BParametros.LkGe3000BNumContratoTerc = 0;
            LkGe3000BParametros.LkGe3000BNumContrato = 0;
            LkGe3000BParametros.LkGe3000BNumItemMov = 0;


            /* PERFORM PMONITOR-CALL-GE3000B THRU PMONITOR-CALL-GE3000B-EXIT */
            Szemb188Pmonitorcallge3000b(); Szemb188Pmonitorcallge3000bexit();


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188Pmonitorgravamonitor Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188Pmonitorgravaarquivos()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188Pmonitorgravaarquivos");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE 'PMONG'                  TO W-LABEL
                                                        W-GE3000B-PARAGRAFO-ORIG */
            WLabel = Utility.PadStrToSize("PMONG", 5);
            WGe3000BParagrafoOrig = Utility.PadStrToSize("PMONG", 5);


            /* MOVE 'GRA'                    TO LK-GE3000B-OPERACAO */
            LkGe3000BParametros.LkGe3000BOperacao = Utility.PadStrToSize("GRA", 3);


            /* MOVE 'G'                      TO LK-GE3000B-PROC-ARQ */
            LkGe3000BParametros.LkGe3000BProcArq = Utility.PadStrToSize("G", 1);


            /* PERFORM PMONITOR-CALL-GE3000B THRU PMONITOR-CALL-GE3000B-EXIT */
            Szemb188Pmonitorcallge3000b(); Szemb188Pmonitorcallge3000bexit();


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188Pmonitorgravaarquivos Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188Pmonitoratualizamonitor()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188Pmonitoratualizamonitor");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE 'PMATM'                  TO W-LABEL
                                                        W-GE3000B-PARAGRAFO-ORIG */
            WLabel = Utility.PadStrToSize("PMATM", 5);
            WGe3000BParagrafoOrig = Utility.PadStrToSize("PMATM", 5);


            /* IF  W-GE3000B-MSG-ERRO-INI = SPACES
                       AND W-GE3000B-MSG-ERRO-GRA = SPACES
                          MOVE 'F'                   TO LK-GE3000B-PROC-ARQ
                          PERFORM PMONITOR-CALL-GE3000B THRU
                                  PMONITOR-CALL-GE3000B-EXIT
                       END-IF */
            if (Utility.CompareWithSpaces(WGe3000BMsgErroIni) == 0 && Utility.CompareWithSpaces(WGe3000BMsgErroGra) == 0)
            {
                LkGe3000BParametros.LkGe3000BProcArq = Utility.PadStrToSize("F", 1);


                Szemb188Pmonitorcallge3000b(); Szemb188Pmonitorcallge3000bexit();


            }


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188Pmonitoratualizamonitor Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188Pmonitorfinalizacao()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188Pmonitorfinalizacao");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* IF  W-GE3000B-MSG-ERRO-INI = SPACES
                       AND W-GE3000B-MSG-ERRO-GRA = SPACES
                          MOVE 'FIN'                 TO LK-GE3000B-OPERACAO
                          PERFORM PMONITOR-CALL-GE3000B THRU
                                  PMONITOR-CALL-GE3000B-EXIT
                       END-IF */
            if (Utility.CompareWithSpaces(WGe3000BMsgErroIni) == 0 && Utility.CompareWithSpaces(WGe3000BMsgErroGra) == 0)
            {
                LkGe3000BParametros.LkGe3000BOperacao = Utility.PadStrToSize("FIN", 3);


                Szemb188Pmonitorcallge3000b(); Szemb188Pmonitorcallge3000bexit();


            }


            /* DISPLAY ' ' */
            Console.WriteLine(" ");


            /* DISPLAY '%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%'
                               '%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%' */
            Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%" + "%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");


            /* IF  W-GE3000B-MSG-ERRO-INI = SPACES
                       AND W-GE3000B-MSG-ERRO-GRA = SPACES
                       AND W-GE3000B-MSG-ERRO-FIN = SPACES
                          DISPLAY 'MONITORACAO DE ARQUIVOS FINALIZADA OK'
                       ELSE
                          IF W-GE3000B-MSG-ERRO-INI NOT = SPACES
                             DISPLAY 'ERRO NO RETORNO DA GE3000B OPERACAO "INI"'
                             DISPLAY 'IND-ERRO = '  W-GE3000B-IND-ERRO-INI
                             DISPLAY '<' W-GE3000B-MSG-ERRO-INI '>'
                          END-IF
                          IF W-GE3000B-MSG-ERRO-GRA NOT = SPACES
                             DISPLAY 'ERRO NO RETORNO DA GE3000B OPERACAO "GRA"'
                             DISPLAY 'IND-ERRO = '  W-GE3000B-IND-ERRO-GRA
                             DISPLAY '<' W-GE3000B-MSG-ERRO-GRA '>'
                          END-IF
                          IF W-GE3000B-MSG-ERRO-FIN NOT = SPACES
                             DISPLAY 'ERRO NO RETORNO DA GE3000B OPERACAO "FIN"'
                             DISPLAY 'IND-ERRO = '  W-GE3000B-IND-ERRO-FIN
                             DISPLAY '<' W-GE3000B-MSG-ERRO-FIN '>'
                          END-IF
                       END-IF */
            if (Utility.CompareWithSpaces(WGe3000BMsgErroIni) == 0 && (Utility.CompareWithSpaces(WGe3000BMsgErroGra) == 0 && Utility.CompareWithSpaces(WGe3000BMsgErroFin) == 0))
            {
                Console.WriteLine("MONITORACAO DE ARQUIVOS FINALIZADA OK");


            }
            else
            {
                if (Utility.CompareWithSpaces(WGe3000BMsgErroIni) != 0)
                {
                    Console.WriteLine("ERRO NO RETORNO DA GE3000B OPERACAO \"INI\"");


                    Console.WriteLine("IND-ERRO = " + WGe3000BIndErroIni);


                    Console.WriteLine("<" + WGe3000BMsgErroIni + ">");


                }



                if (Utility.CompareWithSpaces(WGe3000BMsgErroGra) != 0)
                {
                    Console.WriteLine("ERRO NO RETORNO DA GE3000B OPERACAO \"GRA\"");


                    Console.WriteLine("IND-ERRO = " + WGe3000BIndErroGra);


                    Console.WriteLine("<" + WGe3000BMsgErroGra + ">");


                }



                if (Utility.CompareWithSpaces(WGe3000BMsgErroFin) != 0)
                {
                    Console.WriteLine("ERRO NO RETORNO DA GE3000B OPERACAO \"FIN\"");


                    Console.WriteLine("IND-ERRO = " + WGe3000BIndErroFin);


                    Console.WriteLine("<" + WGe3000BMsgErroFin + ">");


                }



            }


            /* DISPLAY '%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%'
                               '%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%' */
            Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%" + "%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");


            /* DISPLAY ' ' */
            Console.WriteLine(" ");


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188Pmonitorfinalizacao Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188Pmonitorcallge3000b()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188Pmonitorcallge3000b");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* MOVE 'PMCAL'                  TO W-GE3000B-PARAGRAFO-ORIG */
            WGe3000BParagrafoOrig = Utility.PadStrToSize("PMCAL", 5);


            /* IF LK-GE3000B-OPERACAO = 'INI'
                          INITIALIZE LK-GE3000B-SEQ-MONITOR
                                     W-GE3000B-MSG-ERRO-INI
                       ELSE
                          INITIALIZE W-GE3000B-MSG-ERRO-FIN
                       END-IF */
            if ((Comparisions.SafeStringCompare(LkGe3000BParametros.LkGe3000BOperacao, "INI")))
            {
                LkGe3000BParametros.LkGe3000BSeqMonitor = 0;
                WGe3000BMsgErroIni = Utility.PadLeft("", 120, " ");


            }
            else
            {
                WGe3000BMsgErroFin = Utility.PadLeft("", 120, " ");



            }


            /* INITIALIZE LK-GE3000B-IND-ERRO
                                  LK-GE3000B-MSG-RETORNO
                                  W-GE3000B-RETURN-CODE
                                  W-GE3000B-MSG-ERRO */
            LkGe3000BParametros.LkGe3000BIndErro = 0;
            LkGe3000BParametros.LkGe3000BMsgRetorno = Utility.PadLeft("", 70, " ");
            WGe3000BReturnCode = 0;
            WGe3000BMsgErro = Utility.PadLeft("", 120, " ");


            /* MOVE 'GE3000B'                TO W-GE3000B-PROG-CALL */
            WGe3000BProgCall = Utility.PadStrToSize("GE3000B", 8);


            /* CALL W-GE3000B-PROG-CALL USING LK-GE3000B-PARAMETROS */
            Utility.InvokeMethod(this.GetType().Namespace + "." + SerializerUtils.Serialize(WGe3000BProgCall).Trim(), "Execute", new object[] { LkGe3000BParametros });


            /* MOVE RETURN-CODE              TO W-GE3000B-RETURN-CODE */
            WGe3000BReturnCode = Math.Abs(Convert.ToInt64(RETURN_CODE));


            /* IF W-GE3000B-RETURN-CODE NOT = 0
                          DISPLAY W-PROGRAMA '-' W-GE3000B-PARAGRAFO-ORIG
                          DISPLAY '>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>'
                          DISPLAY LK-GE3000B-COD-PROGRAMA
                                  '-RETORNOU DO CALL ' W-GE3000B-PROG-CALL
                                  '. RC=' W-GE3000B-RETURN-CODE
                          DISPLAY 'OPERACAO='        LK-GE3000B-OPERACAO
                          DISPLAY 'COD-USUARIO='     LK-GE3000B-COD-USUARIO
                          DISPLAY 'COD-PROGRAMA='    LK-GE3000B-COD-PROGRAMA
                          DISPLAY 'SEQ-MONITOR='     LK-GE3000B-SEQ-MONITOR
                          DISPLAY 'IND-ERRO='        LK-GE3000B-IND-ERRO
                          DISPLAY 'MSG-RETORNO='     LK-GE3000B-MSG-RETORNO
                          DISPLAY '<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<'
                       END-IF */
            if (WGe3000BReturnCode != 0)
            {
                Console.WriteLine(WPrograma + "-" + WGe3000BParagrafoOrig);


                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");


                Console.WriteLine(LkGe3000BParametros.LkGe3000BCodPrograma + "-RETORNOU DO CALL " + WGe3000BProgCall + ". RC=" + WGe3000BReturnCode);


                Console.WriteLine("OPERACAO=" + LkGe3000BParametros.LkGe3000BOperacao);


                Console.WriteLine("COD-USUARIO=" + LkGe3000BParametros.LkGe3000BCodUsuario);


                Console.WriteLine("COD-PROGRAMA=" + LkGe3000BParametros.LkGe3000BCodPrograma);


                Console.WriteLine("SEQ-MONITOR=" + LkGe3000BParametros.LkGe3000BSeqMonitor);


                Console.WriteLine("IND-ERRO=" + LkGe3000BParametros.LkGe3000BIndErro);


                Console.WriteLine("MSG-RETORNO=" + LkGe3000BParametros.LkGe3000BMsgRetorno);


                Console.WriteLine("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");


            }


            /* IF W-GE3000B-RETURN-CODE NOT = 0
                          STRING LK-GE3000B-COD-PROGRAMA
                                 '-ERRO NO CALL ' W-GE3000B-PROG-CALL '. '
                                 'RC=' W-GE3000B-RETURN-CODE
                                 DELIMITED BY SIZE INTO W-GE3000B-MSG-ERRO
                          INITIALIZE LK-GE3000B-IND-ERRO
                       END-IF */
            if (WGe3000BReturnCode != 0)
            {
                StringBuilder builder1 = new StringBuilder();
                builder1.Append(LkGe3000BParametros.LkGe3000BCodPrograma).Append("-ERRO NO CALL ").Append(WGe3000BProgCall).Append(". ").Append("RC=").Append(SerializerUtils.Serialize(WGe3000BReturnCode, (DataTypeMetadata)DataTypeMap["WGe3000BReturnCode"]));
                string builderOut1 = builder1.ToString();
                if (builderOut1.Length > 120)
                {
                    builderOut1 = builderOut1.SafeSubstring(0, 120);
                }
                WGe3000BMsgErro = builderOut1;


                LkGe3000BParametros.LkGe3000BIndErro = 0;


            }


            /* IF LK-GE3000B-IND-ERRO NOT = 0
                          MOVE 1                     TO W-GE3000B-COUNT
                          INSPECT FUNCTION REVERSE (LK-GE3000B-MSG-RETORNO)
                                  TALLYING W-GE3000B-COUNT FOR LEADING SPACE
                          COMPUTE W-GE3000B-COUNT
                                         = FUNCTION LENGTH(LK-GE3000B-MSG-RETORNO)
                                         + 1
                          MOVE LK-GE3000B-IND-ERRO TO W-GE3000B-IND-ERRO-INI
                          STRING W-PROGRAMA '-' W-GE3000B-PARAGRAFO-ORIG
                                   '-' LK-GE3000B-MSG-RETORNO(1:W-GE3000B-COUNT)
                                 DELIMITED BY SIZE INTO W-GE3000B-MSG-ERRO
                       END-IF */
            if (LkGe3000BParametros.LkGe3000BIndErro != 0)
            {
                WGe3000BCount = Math.Abs(Convert.ToInt64(1));


                WGe3000BCount = Utility.GetLeadingCountOfSequence(SerializerUtils.Serialize(Utility.ReverseString(LkGe3000BParametros.LkGe3000BMsgRetorno)), " ");


                WGe3000BCount = Math.Abs(Utility.Truncate(LkGe3000BParametros.LkGe3000BMsgRetorno.Length + 1));


                WGe3000BIndErroIni = Math.Abs(Convert.ToInt64(LkGe3000BParametros.LkGe3000BIndErro));


                StringBuilder builder2 = new StringBuilder();
                builder2.Append(WPrograma).Append("-").Append(WGe3000BParagrafoOrig).Append("-").Append(Utility.RefMod(LkGe3000BParametros.LkGe3000BMsgRetorno, (DataTypeMetadata)LkGe3000BParametros.DataTypeMap["LkGe3000BMsgRetorno"], 0, (int)WGe3000BCount));
                string builderOut2 = builder2.ToString();
                if (builderOut2.Length > 120)
                {
                    builderOut2 = builderOut2.SafeSubstring(0, 120);
                }
                WGe3000BMsgErro = builderOut2;


            }


            /* EVALUATE LK-GE3000B-OPERACAO
                       WHEN 'INI'
                          MOVE W-GE3000B-MSG-ERRO  TO W-GE3000B-MSG-ERRO-INI
                       WHEN 'GRA'
                          MOVE W-GE3000B-MSG-ERRO  TO W-GE3000B-MSG-ERRO-GRA
                       WHEN 'FIN'
                          MOVE W-GE3000B-MSG-ERRO  TO W-GE3000B-MSG-ERRO-FIN
                       END-EVALUATE */
            if ((Comparisions.SafeStringCompare(LkGe3000BParametros.LkGe3000BOperacao, "INI")))
            {
                WGe3000BMsgErroIni = Utility.PadStrToSize(WGe3000BMsgErro, 120);


            }
            else if ((Comparisions.SafeStringCompare(LkGe3000BParametros.LkGe3000BOperacao, "GRA")))
            {
                WGe3000BMsgErroGra = Utility.PadStrToSize(WGe3000BMsgErro, 120);


            }
            else if ((Comparisions.SafeStringCompare(LkGe3000BParametros.LkGe3000BOperacao, "FIN")))
            {
                WGe3000BMsgErroFin = Utility.PadStrToSize(WGe3000BMsgErro, 120);


            }


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188Pmonitorcallge3000b Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        private void Szemb188Pmonitorcallge3000bexit()
        {
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Entering Szemb188Pmonitorcallge3000bexit");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /* EXIT */
            return;


            stopWatch.Stop();
            LogUtils.WriteLine($"[ecid: {UserSession.SessionInfo.ECID}] Exiting Szemb188Pmonitorcallge3000bexit Method Execution time = {stopWatch.ElapsedMilliseconds} ms");
        }

        public bool Szemb188Wge3000bcttraceon()
        {
            return "SIM".Equals(WGe3000BCtTrace);
        }

        public bool Szemb188Wge3000bcttracedeton()
        {
            return "SIM".Equals(WGe3000BCtTrace);
        }

        public void Initialize()
        {
            RegArqtemp1 = Utility.PadLeft("", 520, " ");
            WPrograma = Utility.PadLeft("", 8, " ");
            WReturnCode = 0;
            WProgramaVersao = Utility.PadLeft("", 80, " ");
            WLabel = Utility.PadLeft("", 5, " ");
            WCall = Utility.PadLeft("", 8, " ");
            WOpenArq = Utility.PadLeft("", 3, " ");
            WFimCertif = Utility.PadLeft("", 3, " ");
            WFimPactuante = Utility.PadLeft("", 3, " ");
            WAtualObjAcop = 0;
            WAtualObjAcopAss = 0;
            WSzemb188Trace = Utility.PadLeft("", 8, " ");
            WSeqRegistro = 0;
            WMensagemErro = Utility.PadLeft("", 500, " ");
            WQtdDisplayTrace = 0;
            WQtdDisplayLog = 0;
            WIntegerValorMax = 0;
            WCodPessoa = 0;
            WNumContratoTerc = 0;
            WNumContratoAnt = 0;
            WNumContrApolice = 0;
            WNumContrato = 0;
            WNumOcupacao = 0;
            WDdi = 0;
            WTotGravados = 0;
            WTotLidosCertComp = 0;
            WTotLidosCertPact = 0;
            WQtdCompra1 = 0;
            WQtdCompra2 = 0;
            WQtdCancel = 0;
            WsDtRef = Utility.PadLeft("", 10, " ");
            WsDtIni = Utility.PadLeft("", 10, " ");
            WsDtFim = Utility.PadLeft("", 10, " ");
            ESqlcode = 0;
            ESmallint1 = 0;
            EInteger1 = 0;
            EInteger2 = 0;
            EInteger3 = 0;
            EInteger4 = 0;
            EBigint1 = 0;
            HDtaCurrent = Utility.PadLeft("", 10, " ");
            HNumVersaoSerie = 0;
            HCodProdutoMin = 0;
            HCodProdutoMax = 0;
            HCodAcopladoMin = 0;
            HCodAcopladoMax = 0;
            HDtaIniVigenciaAtual = Utility.PadLeft("", 10, " ");
            HDtaIniVigenciaOrig = Utility.PadLeft("", 10, " ");
            HDtaFimCarenciaOrig = Utility.PadLeft("", 10, " ");
            NSz012NumContratoAnt = 0;
            Ftm01HRegHeader.Initialize();
            Ftm01DRegDetalhe.Initialize();
            Ftm01TRegTrailler.Initialize();
            Szemb188Parametros.Initialize();
            DclszPessoa.Initialize();
            DclszApolice.Initialize();
            DclszOrigemContrato.Initialize();
            DclszContrTerc.Initialize();
            DclszContrSeguro.Initialize();
            DclszObjAcopladoAssist.Initialize();
            DclszPessoaFisica.Initialize();
            DclszPessoaTelefone.Initialize();
            DclszEndereco.Initialize();
            DclszAcoplado.Initialize();
            DclszApolAcoplado.Initialize();
            DclszObjEndereco.Initialize();
            DclszAcopladoAssist.Initialize();
            DclszObjAcoplado.Initialize();
            Dclcbo.Initialize();
            Sqlca.Initialize();
            HSzl01Szemnl01.Initialize();
            NSzl01Szemnl01.Initialize();
            WGe3000BProgCall = Utility.PadLeft("", 8, " ");
            WGe3000BParagrafoOrig = Utility.PadLeft("", 5, " ");
            WGe3000BReturnCode = 0;
            WGe3000BFimArq = Utility.PadLeft("", 3, " ");
            WGe3000BCount = 0;
            WGe3000BIndErroIni = 0;
            WGe3000BIndErroGra = 0;
            WGe3000BIndErroFin = 0;
            WGe3000BMsgErroIni = Utility.PadLeft("", 120, " ");
            WGe3000BMsgErroGra = Utility.PadLeft("", 120, " ");
            WGe3000BMsgErroFin = Utility.PadLeft("", 120, " ");
            WGe3000BMsgErro = Utility.PadLeft("", 120, " ");
            WGe3000BCtTrace = Utility.PadLeft("", 3, " ");
            LkGe3000BParametros.Initialize();
            Szemb188FileParametros.Initialize();
        }



    }
}