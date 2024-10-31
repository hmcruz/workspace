       IDENTIFICATION DIVISION.
       PROGRAM-ID.    SZEMB188.
       AUTHOR.        DIOGO MATHEUS.
       DATE-WRITTEN.  08/11/2022.
      ******************************************************************
      * SMART - SISTEMA DE ADMINISTRACAO DE CONTRATOS DE SEGURO DE     *
      *         CREDITO FINANCEIRO                                     *
      *----------------------------------------------------------------*
      * PROGRAMA : SZEMB188.                                           *
      * DESCRICAO: GERA ARQUIVO ACOPLADO SAF (TEMPO ASSIST) MENSAL     *
      *----------------------------------------------------------------*
      * HISTORICO DE CORRECOES                                         *
      *----------------------------------------------------------------*
      * NO VERSAO DATA       RESPONSAVEL      ALTERACAO                *
      *----------------------------------------------------------------*
      *----------------------------------------------------------------*
V.2   * VERSAO  : 002
      * MOTIVO  : AJUSTE NO ENVIO DT INICIO/FIM DE VIGENCIA DO CONTRATO
      * JAZZ    : 441629
      * DATA    : 20/12/2022
      * NOME    : DIOGO MATHEUS
      * MARCADOR: V.2
      *----------------------------------------------------------------*
      *----------------------------------------------------------------*
V.1   * VERSAO  : 001
      * MOTIVO  : ARQUIVO SAF (TEMPO ASSIST) MENSAL
      * JAZZ    : 441629
      * DATA    : 08/11/2022
      * NOME    : DIOGO MATHEUS
      * MARCADOR: V.1
      *----------------------------------------------------------------*
       ENVIRONMENT DIVISION.
       CONFIGURATION SECTION.
       SPECIAL-NAMES.
           DECIMAL-POINT         IS    COMMA.
       INPUT-OUTPUT SECTION.
       FILE-CONTROL.
      *
           SELECT ARQTEMP1       ASSIGN TO ARQTEMP1.
      *
       DATA DIVISION.
       FILE SECTION.
      *
       FD   ARQTEMP1
            LABEL     RECORD     IS  OMITTED
            RECORDING MODE       IS  F
            RECORD    CONTAINS   520 CHARACTERS.
       01   REG-ARQTEMP1             PIC  X(520).
      ******************************************************************
       WORKING-STORAGE SECTION.
      ******************************************************************
      *----------------------------------------------------------------*
      * --- AREAS DE CONTROLE
      *----------------------------------------------------------------*
       77 W-PROGRAMA                 PIC  X(008) VALUE 'SZEMB188'.
       77 W-RETURN-CODE              PIC  9(006) VALUE 0.
       77 W-PROGRAMA-VERSAO          PIC  X(080) VALUE SPACES.
       77 W-LABEL                    PIC  X(005) VALUE SPACES.
       77 W-CALL                     PIC  X(008) VALUE SPACES.
       77 W-OPEN-ARQ                 PIC  X(003) VALUE 'NAO'.
       77 W-FIM-CERTIF               PIC  X(003) VALUE SPACES.
       77 W-FIM-PACTUANTE            PIC  X(003) VALUE SPACES.
       77 W-ATUAL-OBJ-ACOP           PIC  9(009) VALUE ZEROS.
       77 W-ATUAL-OBJ-ACOP-ASS       PIC  9(009) VALUE ZEROS.
       77 W-SZEMB188-TRACE           PIC  X(008).
       77 W-SEQ-REGISTRO             PIC  9(009) VALUE 0.
       77 W-MENSAGEM-ERRO            PIC  X(500) VALUE SPACES.
       77 W-QTD-DISPLAY-TRACE        PIC  9(008) VALUE ZEROS.
       77 W-QTD-DISPLAY-LOG          PIC  9(008) VALUE ZEROS.

      *----------------------------------------------------------------*
      * --- AREAS AUXILIARES
      *----------------------------------------------------------------*
       77 W-INTEGER-VALOR-MAX        PIC  S9(09) COMP.
       77 W-COD-PESSOA               PIC  9(010).
       77 W-NUM-CONTRATO-TERC        PIC  9(018) VALUE ZEROS.
       77 W-NUM-CONTRATO-ANT         PIC  9(018) VALUE ZEROS.
       77 W-NUM-CONTR-APOLICE        PIC  9(018) VALUE ZEROS.
       77 W-NUM-CONTRATO             PIC  9(009) VALUE ZEROS.
       77 W-NUM-OCUPACAO             PIC  9(009) VALUE ZEROS.
       77 W-DDI                      PIC S9(004) USAGE COMP.

      *----------------------------------------------------------------*
      * --- CONTADORES
      *----------------------------------------------------------------*
       77 W-TOT-GRAVADOS             PIC  9(006) VALUE ZEROS.
       77 W-TOT-LIDOS-CERT-COMP      PIC  9(008) VALUE ZEROS.
       77 W-TOT-LIDOS-CERT-PACT      PIC  9(008) VALUE ZEROS.
       77 W-QTD-COMPRA1              PIC  9(008) VALUE ZEROS.
       77 W-QTD-COMPRA2              PIC  9(008) VALUE ZEROS.
       77 W-QTD-CANCEL               PIC  9(008) VALUE ZEROS.
       77 WS-DT-REF                  PIC  X(010) VALUE SPACES.
       77 WS-DT-INI                  PIC  X(010) VALUE SPACES.
       77 WS-DT-FIM                  PIC  X(010) VALUE SPACES.

      *----------------------------------------------------------------*
      * --- AREAS DE EDICAO
      *----------------------------------------------------------------*
       77 E-SQLCODE                  PIC  Z999-.
       77 E-SMALLINT-1               PIC -----9.
       77 E-INTEGER-1                PIC ----------9.
       77 E-INTEGER-2                PIC ----------9.
       77 E-INTEGER-3                PIC ----------9.
       77 E-INTEGER-4                PIC ----------9.
       77 E-BIGINT-1                 PIC 9(018).

      *----------------------------------------------------------------*
      * DB2 - VARIAVEIS HOSTS
      *----------------------------------------------------------------*
       01 H-DTA-CURRENT              PIC  X(010).
       01 H-NUM-VERSAO-SERIE         PIC S9(004) USAGE COMP.
       01 H-COD-PRODUTO-MIN          PIC  S9(09) COMP.
       01 H-COD-PRODUTO-MAX          PIC  S9(09) COMP.
       01 H-COD-ACOPLADO-MIN         PIC  S9(09) COMP.
       01 H-COD-ACOPLADO-MAX         PIC  S9(09) COMP.
       01 H-DTA-INI-VIGENCIA-ATUAL   PIC  X(010).
       01 H-DTA-INI-VIGENCIA-ORIG    PIC  X(010).
       01 H-DTA-FIM-CARENCIA-ORIG    PIC  X(010).

      *----------------------------------------------------------------*
      * VARIAVEIS INDICADORAS DE NULO
      *----------------------------------------------------------------*
       01 N-SZ012-NUM-CONTRATO-ANT   PIC S9(004) COMP.

      *----------------------------------------------------------------*
      * CABECALHO ARQUIVO PARA TEMPO
      *----------------------------------------------------------------*
       01 FTM01-H-REG-HEADER.
          03 FTM01-H-COD-CLIENTE     PIC  9(003) VALUE ZEROS.
          03 FTM01-H-COD-PRODUTO     PIC  9(002) VALUE ZEROS.
          03 FTM01-H-DTA-GERACAO     PIC  9(008) VALUE ZEROS.
          03 FTM01-H-NUM-SEQ         PIC  9(005) VALUE ZEROS.
          03 FTM01-H-FILLER          PIC  X(502) VALUE SPACES.
      * DETALHE ARQUIVO PARA TEMPO
       01 FTM01-D-REG-DETALHE.
          03 FTM01-D-NUM-APOLICE     PIC  X(040) VALUE SPACES.
          03 FTM01-D-NUM-ITEM        PIC  X(005) VALUE SPACES.
          03 FTM01-D-NUM-CARTAO      PIC  X(030) VALUE SPACES.
          03 FTM01-D-COD-SEGURO      PIC  9(010) VALUE ZEROS.
          03 FTM01-D-SIT-REGISTRO    PIC  X(001) VALUE SPACES.
          03 FTM01-D-DTA-INICIO      PIC  9(008) VALUE ZEROS.
          03 FTM01-D-DTA-FIM         PIC  9(008) VALUE ZEROS.
          03 FTM01-D-NOM-SEGURADO    PIC  X(040) VALUE SPACES.
          03 FTM01-D-CPF-CNPJ        PIC  9(014) VALUE ZEROS.
          03 FTM01-D-END-SEGURADO    PIC  X(060) VALUE SPACES.

          03 FTM01-D-NUM-ENDERECO    PIC  X(006) VALUE SPACES.
          03 FTM01-D-END-COMPLEME    PIC  X(040) VALUE SPACES.
          03 FTM01-D-NOM-CIDADE      PIC  X(062) VALUE SPACES.
          03 FTM01-D-NOM-BAIRRO      PIC  X(040) VALUE SPACES.
          03 FTM01-D-CEP-CIDADE      PIC  9(008) VALUE ZEROS.
          03 FTM01-D-NOM-PAIS        PIC  X(020) VALUE SPACES.
          03 FTM01-D-DDI-TEL         PIC  9(003) VALUE ZEROS.
          03 FTM01-D-DDD-TEL         PIC  9(003) VALUE ZEROS.
          03 FTM01-D-NUM-TEL         PIC  9(010) VALUE ZEROS.
          03 FTM01-D-COD-SEXO        PIC  9(001) VALUE ZEROS.
          03 FTM01-D-DTA-NASC        PIC  9(008) VALUE ZEROS.
          03 FTM01-D-COD-RG          PIC  9(018) VALUE ZEROS.
          03 FTM01-D-EST-CIVIL       PIC  9(001) VALUE ZEROS.
          03 FTM01-D-NOM-PROFISSAO   PIC  X(060) VALUE SPACES.
          03 FTM01-D-DTA-INI-VIG-SEG PIC  9(008) VALUE ZEROS.
          03 FTM01-D-DTA-FIM-VIG-SEG PIC  9(008) VALUE ZEROS.
          03 FTM01-D-FILLER          PIC  X(008) VALUE SPACES.
      * TRAILER ARQUIVO PARA TEMPO
       01 FTM01-T-REG-TRAILLER.
          03 FTM01-T-QTD-REG         PIC  9(005) VALUE ZEROS.
          03 FTM01-T-SEPARADOR       PIC  X(001) VALUE SPACES.
          03 FTM01-T-IDE-INCL        PIC  X(001) VALUE 'I'.
          03 FTM01-T-QTD-INCL        PIC  9(005) VALUE ZEROS.
          03 FTM01-T-IDE-ALTE        PIC  X(001) VALUE 'A'.
          03 FTM01-T-QTD-ALTE        PIC  9(005) VALUE ZEROS.
          03 FTM01-T-IDE-CANC        PIC  X(001) VALUE 'C'.
          03 FTM01-T-QTD-CANC        PIC  9(005) VALUE ZEROS.
          03 FTM01-T-IDE-REAT        PIC  X(001) VALUE 'R'.
          03 FTM01-T-QTD-REAT        PIC  9(005) VALUE ZEROS.
          03 FTM01-T-FILLER          PIC  X(490) VALUE SPACES.

      *----------------------------------------------------------------*
      *--- AREA DO ARQUIVO DE PARAMETROS
      *----------------------------------------------------------------*
       01       SZEMB188-PARAMETROS.
           03   SZEMB188-COD-PRODUTO-X.
             05 SZEMB188-COD-PRODUTO     PIC 9(006).
           03   SZEMB188-COD-ACOPLADO-X.
             05 SZEMB188-COD-ACOPLADO    PIC 9(009).
           03   FILLER                   PIC X(001).
           03   SZEMB188-TRACE           PIC X(008).
             88 SZEMB188-TRACE-ON-88     VALUE 'TRACE ON'.

      *----------------------------------------------------------------
      * AREA DAS VARIAVEIS HOST
      *----------------------------------------------------------------
      *    --- SZ_PESSOA
           EXEC SQL INCLUDE SZ008    END-EXEC.
      *    --- SZ_APOLICE
           EXEC SQL INCLUDE SZ010    END-EXEC.
      *    --- SZ_ORIGEM_CONTRATO
           EXEC SQL INCLUDE SZ011    END-EXEC.
      *    --- SZ_CONTR_TERC
           EXEC SQL INCLUDE SZ012    END-EXEC.
      *    --- SZ_CONTRATO_SEGURO
           EXEC SQL INCLUDE SZ021    END-EXEC.
      *    --- SZ_OBJ_ACOPLADO_ASSIST
           EXEC SQL INCLUDE SZ043    END-EXEC.
      *    --- SZ_PESSOA_FISICA
           EXEC SQL INCLUDE SZ053    END-EXEC.
      *    --- SZ_PESSOA_TELEFONE
           EXEC SQL INCLUDE SZ057    END-EXEC.
      *    --- SZ_ENDERECO
           EXEC SQL INCLUDE SZ063    END-EXEC.
      *    --- SZ_ACOPLADO
           EXEC SQL INCLUDE SZ072    END-EXEC.
      *    --- SZ_OBJ_ACOPLADO
           EXEC SQL INCLUDE SZ073    END-EXEC.
      *    --- SZ_OBJ_PESSOA
           EXEC SQL INCLUDE SZ077    END-EXEC.
      *    --- SZ_ACOPLADO_ASSIST
           EXEC SQL INCLUDE SZ098    END-EXEC.
      *    --- SZ_OBJ_ACOPLADO
           EXEC SQL INCLUDE SZ115    END-EXEC.
      *    --- CBO
           EXEC SQL INCLUDE CBO      END-EXEC.
      *    --- SQL COMMUNICATION AREA
           EXEC SQL INCLUDE SQLCA    END-EXEC.

      *-----------------------------------------------------------------
      * SZEMWL01 - BOOK PARA CALL DA SZEMNL01 (GRAVA LOG)
      *-----------------------------------------------------------------
      *    --- SP SEGUROS.SZEMNL01
           EXEC SQL INCLUDE SZEMWL01 END-EXEC.

      *-----------------------------------------------------------------
      * PMONWK01 - BOOK COM A DEFINICAO DE AREA USADAS P/ CALL GE3000B
      *-----------------------------------------------------------------
       COPY PMONWK01 REPLACING  ==:GE3000B:==  BY  ==GE3000B==.

      *-----------------------------------------------------------------
      * PMONLK01 - BOOK COM A DEFINICAO DE LINKAGE P/ CALL DO GE3000B
      *-----------------------------------------------------------------
       COPY PMONLK01 REPLACING  ==:GE3000B:==  BY  ==GE3000B==.
      *-----------------------------------------------------------------

      *-----------------------------------------------------------------
      *    CURSOR CR_PACTUANTE
      *-----------------------------------------------------------------
           EXEC SQL DECLARE CR_PACTUANTE CURSOR FOR
             SELECT SZ008.NOM_RAZ_SOCIAL
                  , SZ008.NUM_CPF_CNPJ
                  , SZ053.DTA_NASCIMENTO
                  , SZ008.NUM_PESSOA
                  , SZ053.IND_SEXO
                  , IFNULL(SZ053.IND_ESTADO_CIVIL,'1')
                  , VALUE(SZ053.NUM_OCUPACAO,0) AS CBO
               FROM SEGUROS.SZ_CONTR_TERC SZ012
               JOIN SEGUROS.SZ_OBJ_PESSOA SZ067
                 ON SZ067.NUM_CONTRATO = SZ012.NUM_CONTRATO
               JOIN SEGUROS.SZ_PESSOA SZ008
                 ON SZ008.NUM_PESSOA = SZ067.NUM_PESSOA
               JOIN SEGUROS.SZ_PESSOA_FISICA SZ053
                 ON SZ053.NUM_PESSOA = SZ008.NUM_PESSOA
              WHERE SZ012.NUM_CONTRATO_TERC = :SZ012-NUM-CONTRATO-TERC
               WITH UR
           END-EXEC.

      ******************************************************************
       PROCEDURE DIVISION.
      ******************************************************************
      *----------------------------------------------------------------*
       P0000-PRINCIPAL.
      *----------------------------------------------------------------*
           MOVE 'P0000'                  TO W-LABEL

           DISPLAY ' '
V.2        DISPLAY 'SZEMB188 - VERSAO 002 - INICIOU PROCESSAMENTO EM: '
              FUNCTION CURRENT-DATE(07:2) '/'
              FUNCTION CURRENT-DATE(05:2) '/'
              FUNCTION CURRENT-DATE(01:4) ' AS '
              FUNCTION CURRENT-DATE(09:2) ':'
              FUNCTION CURRENT-DATE(11:2) ':'
              FUNCTION CURRENT-DATE(13:2) ' ***'
           DISPLAY ' '

           PERFORM P1000-INICIALIZA

           PERFORM P2000-PRINCIPAL-COMPRA

      *    --- FINALIZA PROCESSAMENTO
           MOVE  00                      TO  W-RETURN-CODE
           PERFORM P9000-FINALIZA
           GO TO P9999-FINALIZACAO
           .
      *P0000-PRINCIPAL-EXIT. EXIT.
      *
      *----------------------------------------------------------------*
       P1000-INICIALIZA.
      *----------------------------------------------------------------*
           MOVE 'P1000'                  TO W-LABEL

           INITIALIZE W-PROGRAMA-VERSAO
                      W-RETURN-CODE

           STRING W-PROGRAMA '-V.08-393786-ARQUIVO-TEMPO ASSISTENCIA-'
                  FUNCTION WHEN-COMPILED(1:12)
                DELIMITED BY SIZE INTO W-PROGRAMA-VERSAO

           DISPLAY 'PROCESSAMENTO PARA GERAR ARQUIVO PARA A TEMPO.'

           INITIALIZE W-SEQ-REGISTRO

           PERFORM P1200-INICIALIZAR-MONITORACAO

           OPEN OUTPUT ARQTEMP1
           MOVE 'SIM'                    TO W-OPEN-ARQ

           MOVE +2147483647              TO W-INTEGER-VALOR-MAX

           MOVE 0                        TO H-COD-PRODUTO-MIN
           MOVE W-INTEGER-VALOR-MAX      TO H-COD-PRODUTO-MAX

           MOVE 0                        TO H-COD-ACOPLADO-MIN
           MOVE W-INTEGER-VALOR-MAX      TO H-COD-ACOPLADO-MAX

           EXEC SQL
                SELECT CURRENT DATE   - 1 MONTH
                INTO :WS-DT-REF
                FROM SYSIBM.SYSDUMMY1
           END-EXEC

           EXEC SQL
                SELECT DATE(SUBSTR(CHAR(DATE(:WS-DT-REF)),1,7)
                            ||'-01')
                     , DATE(SUBSTR(CHAR(DATE(:WS-DT-REF)
                            + 1 MONTH),1,7)||'-01') - 1 DAY
                INTO :WS-DT-INI
                    ,:WS-DT-FIM
                FROM SYSIBM.SYSDUMMY1
           END-EXEC

           DISPLAY 'WS-DT-INI = ' WS-DT-INI
           DISPLAY 'WS-DT-FIM = ' WS-DT-FIM
           .
      *P1000-INICIALIZA-EXIT. EXIT.

      *----------------------------------------------------------------*
       P1200-INICIALIZAR-MONITORACAO.
      *----------------------------------------------------------------*
           MOVE 'P1200'                  TO W-LABEL

      *    --- INICIALIZA-MONITORACAO ----------------------------------
           INITIALIZE LK-GE3000B-PARAMETROS
                      H-SZL01-SEQ-LOG-SISTEMA
           MOVE W-PROGRAMA               TO LK-GE3000B-COD-PROGRAMA
                                            LK-GE3000B-COD-USUARIO
           IF SZEMB188-TRACE-ON-88
              MOVE 'SIM'                 TO LK-GE3000B-TRACE
           END-IF

           IF SZEMB188-TRACE-ON-88
              DISPLAY W-PROGRAMA '-VAI CALL PMONITOR: PARAMETROS='
                      ' OPERACAO<' LK-GE3000B-OPERACAO '>'
                      ' PROC-ARQ<' LK-GE3000B-PROC-ARQ '>'
              DISPLAY 'P1<' LK-GE3000B-PARAMETROS(001:100) '>'
              DISPLAY 'P2<' LK-GE3000B-PARAMETROS(101:100) '>'
           END-IF

           PERFORM PMONITOR-INICIALIZACAO

      *    --- INICIALIZA ARQUIVOS MONITORADOS (GRAVA DADOS DO ARQUIVO)
           MOVE 'TEM'                    TO LK-GE3000B-COD-TP-ARQUIVO
           MOVE 'ARQTEMP1'               TO LK-GE3000B-ASSIGN-DDNAME
           MOVE 'FTM01'                  TO LK-GE3000B-COD-LEIAUTE
           PERFORM PMONITOR-GRAVA-MONITOR

      *    --- COMITA ATUALIZACOES DA MONITORACAO ----------------------
           PERFORM DB900-EXECUTA-COMMIT
      *    -------------------------------------------------------------
           .
      *P1200-INICIALIZAR-MONITORACAO-EXIT. EXIT.

      *----------------------------------------------------------------*
       P2000-PRINCIPAL-COMPRA.
      *----------------------------------------------------------------*
           MOVE 'P2000'                  TO W-LABEL

           MOVE SPACES                   TO W-FIM-CERTIF

           MOVE H-COD-PRODUTO-MIN        TO E-INTEGER-1
           MOVE H-COD-PRODUTO-MAX        TO E-INTEGER-2
           MOVE H-COD-ACOPLADO-MIN       TO E-INTEGER-3
           MOVE H-COD-ACOPLADO-MAX       TO E-INTEGER-4

      *    --- ABRE CURSOR CR_CERTIF_COMPRA

           PERFORM DB010-OPEN-CR-CERT-COMPRA.

      *    --- PRIMEIRO FETCH CURSOR CR_CERTIF_COMPRA

           PERFORM DB020-FETCH-CERT-COMPRA

           DISPLAY ' '
           DISPLAY W-PROGRAMA '-FEZ PRIMEIRO FETCH CURSOR '
                   'CR_CERTIF_COMPRA AS '  FUNCTION CURRENT-DATE

           PERFORM UNTIL W-FIM-CERTIF = 'SIM'
      *      --- ACESSA PACTUANTE
             PERFORM DB030-ABRIR-PACTUANTE
             PERFORM DB040-LER-PACTUANTE

             IF W-FIM-PACTUANTE = 'SIM'
      *         --- GRAVA LOG
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
      *        --- ACESSA ENDERECO
                PERFORM DB050-LER-ENDERECO

      *        --- ACESSA INICIO DE VIGENCIA
                IF N-SZ012-NUM-CONTRATO-ANT = 0
                AND SZ012-NUM-CONTRATO-ANT NOT = SZ012-NUM-CONTRATO-TERC
                   PERFORM DB060-ACESSA-VIGENCIA-CONTR
                END-IF
             END-IF

      *     --- PROCESSA PACTUANTES

             PERFORM UNTIL W-FIM-PACTUANTE = 'SIM'
      *       --- GERA CABECALHO
               IF W-TOT-GRAVADOS = 0
                  PERFORM P2800-GRAVA-CABECALHO
              END-IF
      *       --- MONTA DETALHE
               PERFORM P2100-TRATAR-SAF
      *       --- GRAVA DETALHE
               PERFORM P2810-GRAVA-DETALHE
      *        --- ATUALIZA ACOPLADO
      *        --- ACESSA PROXIMO PACTUANTE
               PERFORM DB040-LER-PACTUANTE
             END-PERFORM
      *      --- PROXIMO CERTIFICADO
             PERFORM DB020-FETCH-CERT-COMPRA
           END-PERFORM

           IF W-TOT-GRAVADOS > 0
              PERFORM P2820-GRAVA-TRAILLER
           END-IF

           IF W-TOT-LIDOS-CERT-COMP = ZEROS
      *       --- GRAVA LOG
              INITIALIZE W-MENSAGEM-ERRO
              STRING 'NAO FOI ENCONTRADO CERTIFICADO PARA O PRODUTO '
                     SZEMB188-COD-PRODUTO-X '. '
                     DELIMITED BY SIZE INTO W-MENSAGEM-ERRO(001:80)
              STRING 'PRODUTO E/OU CONTRATO SEM DIREITO A CERTIFICADO.'
                     DELIMITED BY SIZE INTO W-MENSAGEM-ERRO(081:80)
              MOVE 0                     TO H-SZL01-IND-ERRO-LOG
              MOVE 0                     TO N-SZL01-IND-ERRO-LOG
              PERFORM C0010-CALL-SP-SZEMNL01
           END-IF
           .
      *P2000-PRINCIPAL-COMPRA-EXIT. EXIT.

      *----------------------------------------------------------------*
       P2100-TRATAR-SAF.
      *----------------------------------------------------------------*
           MOVE 'P2100'                  TO W-LABEL

           STRING
                SZ115-DTA-INI-VIGENCIA(1:4)
                SZ115-DTA-INI-VIGENCIA(6:2)
                SZ115-DTA-INI-VIGENCIA(9:2)
                DELIMITED BY SIZE      INTO FTM01-D-DTA-INICIO
           END-STRING

           STRING
                SZ115-DTA-FIM-VIGENCIA(1:4)
                SZ115-DTA-FIM-VIGENCIA(6:2)
                SZ115-DTA-FIM-VIGENCIA(9:2)
                DELIMITED BY SIZE      INTO FTM01-D-DTA-FIM
           END-STRING

           MOVE SPACES                   TO FTM01-D-NUM-ITEM
           MOVE SZ010-NUM-APOLICE        TO FTM01-D-NUM-CARTAO
           MOVE SZ011-COD-PRODUTO        TO FTM01-D-COD-SEGURO
           MOVE SZ043-IND-ENVIO          TO FTM01-D-SIT-REGISTRO
           MOVE W-NUM-CONTR-APOLICE      TO FTM01-D-NUM-APOLICE

           MOVE SZ063-NOM-LOGRADOURO     TO FTM01-D-END-SEGURADO

           MOVE SZ063-NUM-LOGRADOURO     TO FTM01-D-NUM-ENDERECO

           MOVE SZ063-DES-COMPL-ENDERECO TO FTM01-D-END-COMPLEME
           MOVE SZ063-NOM-CIDADE         TO FTM01-D-NOM-CIDADE
           MOVE SZ063-NOM-BAIRRO         TO FTM01-D-NOM-BAIRRO
           COMPUTE FTM01-D-CEP-CIDADE = FUNCTION NUMVAL-C
                                         (SZ063-COD-CEP)
           MOVE 'BRASIL'                 TO FTM01-D-NOM-PAIS

      *    --- MONTA DADOS DO PACTUANTE

           PERFORM DB070-LER-TELEFONE

           MOVE W-DDI                    TO FTM01-D-DDI-TEL
           MOVE SZ057-NUM-DDD            TO FTM01-D-DDD-TEL
           MOVE SZ057-NUM-TELEFONE       TO FTM01-D-NUM-TEL

           MOVE SZ008-NOM-RAZ-SOCIAL     TO FTM01-D-NOM-SEGURADO
           MOVE SZ008-NUM-CPF-CNPJ       TO FTM01-D-CPF-CNPJ
           PERFORM DB080-DESC-PROFISSAO
           MOVE CBO-DESCR-CBO            TO FTM01-D-NOM-PROFISSAO
           IF SZ053-IND-SEXO = 'M'
              MOVE 1                     TO FTM01-D-COD-SEXO
           ELSE
              MOVE 2                     TO FTM01-D-COD-SEXO
           END-IF
           INITIALIZE FTM01-D-DTA-NASC
           STRING SZ053-DTA-NASCIMENTO(1:4)
                  SZ053-DTA-NASCIMENTO(6:2)
                  SZ053-DTA-NASCIMENTO(9:2)
                DELIMITED BY SIZE INTO FTM01-D-DTA-NASC
           END-STRING
           MOVE ALL '0'                  TO FTM01-D-COD-RG
           MOVE SZ053-IND-ESTADO-CIVIL   TO FTM01-D-EST-CIVIL

V.2        INITIALIZE FTM01-D-DTA-INI-VIG-SEG
V.2*****   INITIALIZE SZ021-DTA-INI-VIG-SEG
           STRING SZ021-DTA-INI-VIG-SEG(1:4)
                  SZ021-DTA-INI-VIG-SEG(6:2)
                  SZ021-DTA-INI-VIG-SEG(9:2)
                DELIMITED BY SIZE INTO FTM01-D-DTA-INI-VIG-SEG

V.2        INITIALIZE FTM01-D-DTA-FIM-VIG-SEG
V.2*****   INITIALIZE SZ021-DTA-FIM-VIG-SEG
           STRING SZ021-DTA-FIM-VIG-SEG(1:4)
                  SZ021-DTA-FIM-VIG-SEG(6:2)
                  SZ021-DTA-FIM-VIG-SEG(9:2)
                DELIMITED BY SIZE INTO FTM01-D-DTA-FIM-VIG-SEG
           .
      *P2100-TRATAR-SAF-EXIT. EXIT.

      *----------------------------------------------------------------*
       P2800-GRAVA-CABECALHO.
      *----------------------------------------------------------------*
           MOVE 'P2800'                  TO W-LABEL

           MOVE 9                        TO FTM01-H-COD-CLIENTE
           MOVE SZ098-COD-PROD-ACOPLADO  TO FTM01-H-COD-PRODUTO
           MOVE FUNCTION CURRENT-DATE(1:8)
                                         TO FTM01-H-DTA-GERACAO
           MOVE SZ098-SEQ-ENVIO          TO FTM01-H-NUM-SEQ
           WRITE REG-ARQTEMP1 FROM FTM01-H-REG-HEADER
      *    -------------------------------------------------------------
      *    --- GRAVA ARQUIVOS MONITORADOS (HEADER)
           ADD 1                         TO W-SEQ-REGISTRO
           MOVE W-SEQ-REGISTRO           TO LK-GE3000B-NUM-ITEM-MOV
           INITIALIZE LK-GE3000B-NUM-PES-OPERADOR
                      LK-GE3000B-NUM-LINHA-PRODUTO
                      LK-GE3000B-NUM-CONTRATO-TERC
                      LK-GE3000B-NUM-CONTRATO
           MOVE 'H'                      TO LK-GE3000B-COD-TP-REGISTRO
           MOVE FTM01-H-REG-HEADER       TO LK-GE3000B-TXT-CONTD
           PERFORM PMONITOR-GRAVA-ARQUIVOS
      *    -------------------------------------------------------------
           .
      *P2800-GRAVA-CABECALHO-EXIT. EXIT.

      *----------------------------------------------------------------*
       P2810-GRAVA-DETALHE.
      *----------------------------------------------------------------*
           MOVE 'P2810'                  TO W-LABEL

           ADD 1                         TO W-TOT-GRAVADOS
           WRITE REG-ARQTEMP1   FROM FTM01-D-REG-DETALHE
      *    -------------------------------------------------------------
      *    --- GRAVA ARQUIVOS MONITORADOS (DETALHE)
           ADD 1                         TO W-SEQ-REGISTRO
           MOVE W-SEQ-REGISTRO           TO LK-GE3000B-NUM-ITEM-MOV
           MOVE SZ012-NUM-PES-OPERADOR   TO LK-GE3000B-NUM-PES-OPERADOR
           MOVE SZ012-NUM-LINHA-PRODUTO  TO LK-GE3000B-NUM-LINHA-PRODUTO
           MOVE SZ012-NUM-CONTRATO-TERC  TO LK-GE3000B-NUM-CONTRATO-TERC
           MOVE SZ012-NUM-CONTRATO       TO LK-GE3000B-NUM-CONTRATO
           MOVE 'D'                      TO LK-GE3000B-COD-TP-REGISTRO
           MOVE FTM01-D-REG-DETALHE      TO LK-GE3000B-TXT-CONTD
           PERFORM PMONITOR-GRAVA-ARQUIVOS
      *    -------------------------------------------------------------

           IF SZ043-IND-ENVIO = 'I'
              ADD   1                    TO W-QTD-COMPRA1
           ELSE
              ADD   1                    TO W-QTD-COMPRA2
           END-IF
           .
      *P2810-GRAVA-DETALHE-EXIT. EXIT.

      *----------------------------------------------------------------*
       P2820-GRAVA-TRAILLER.
      *----------------------------------------------------------------*
           MOVE 'P2820'                  TO W-LABEL

           MOVE W-QTD-COMPRA1            TO FTM01-T-QTD-INCL
           MOVE W-QTD-COMPRA2            TO FTM01-T-QTD-ALTE
           MOVE W-QTD-CANCEL             TO FTM01-T-QTD-CANC
           COMPUTE FTM01-T-QTD-REG = FTM01-T-QTD-INCL
                                      + FTM01-T-QTD-CANC

           WRITE REG-ARQTEMP1 FROM FTM01-T-REG-TRAILLER
      *    -------------------------------------------------------------
      *    --- GRAVA ARQUIVOS MONITORADOS (TRAILLER)
           ADD 1                         TO W-SEQ-REGISTRO
           MOVE W-SEQ-REGISTRO           TO LK-GE3000B-NUM-ITEM-MOV
           INITIALIZE LK-GE3000B-NUM-PES-OPERADOR
                      LK-GE3000B-NUM-LINHA-PRODUTO
                      LK-GE3000B-NUM-CONTRATO-TERC
                      LK-GE3000B-NUM-CONTRATO
           MOVE 'T'                      TO LK-GE3000B-COD-TP-REGISTRO
           MOVE FTM01-T-REG-TRAILLER     TO LK-GE3000B-TXT-CONTD
           PERFORM PMONITOR-GRAVA-ARQUIVOS
      *    -------------------------------------------------------------
           .
      *P2820-GRAVA-TRAILLER-EXIT. EXIT.

      *----------------------------------------------------------------*
       C0010-CALL-SP-SZEMNL01.
      *----------------------------------------------------------------*
      *    --- MOSTRA MENSAGEM PARA IND-ERRO-LOG = 0
           IF H-SZL01-IND-ERRO-LOG = 0
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
           END-IF

      *    --- MONTA PARAMETROS DE ENTRADA
           MOVE W-PROGRAMA               TO H-SZL01-COD-USUARIO
                                           H-SZL01-COD-PROGRAMA
           INITIALIZE N-SZL01-COD-USUARIO
                      N-SZL01-COD-PROGRAMA
      *    --- MONTA DES-MSG-SISTEMA
           INITIALIZE H-SZL01-DES-MSG-SISTEMA-T
                      H-SZL01-DES-MSG-SISTEMA-L
           IF H-SZL01-IND-ERRO-LOG = 0
              STRING W-PROGRAMA '-' W-LABEL '-AVISO: ' W-MENSAGEM-ERRO
                    DELIMITED BY SIZE INTO H-SZL01-DES-MSG-SISTEMA-T
           ELSE
              STRING W-PROGRAMA '-' W-LABEL '-ERRO: ' W-MENSAGEM-ERRO
                    DELIMITED BY SIZE INTO H-SZL01-DES-MSG-SISTEMA-T
           END-IF
           INSPECT FUNCTION REVERSE(H-SZL01-DES-MSG-SISTEMA-T)
                   TALLYING         H-SZL01-DES-MSG-SISTEMA-L
                   FOR LEADING ' '
           INITIALIZE N-SZL01-DES-MSG-SISTEMA
      *    ---
           IF H-SZL01-IND-ERRO-LOG = 2
              MOVE SQLCODE               TO H-SZL01-SQLCODE-LOG
              MOVE SQLERRMC              TO H-SZL01-SQLERRMC-LOG
           ELSE
             INITIALIZE H-SZL01-SQLCODE-LOG
                        H-SZL01-SQLERRMC-LOG
           END-IF
           INITIALIZE N-SZL01-SQLCODE-LOG
                     N-SZL01-SQLERRMC-LOG
           MOVE W-MENSAGEM-ERRO          TO H-SZL01-DES-MSG-RETORNO
           INITIALIZE N-SZL01-DES-MSG-RETORNO

      *    --- INICIALIZA PARAMETROS DE SAIDA
           INITIALIZE H-SZL01-SEQ-LOG-SISTEMA
                      H-SZL01-IND-ERRO
                      H-SZL01-MSG-RET
                      H-SZL01-NM-TAB
                      H-SZL01-SQLCODE
                      H-SZL01-SQLERRMC
           MOVE -1                       TO N-SZL01-SEQ-LOG-SISTEMA
                                            N-SZL01-IND-ERRO
                                            N-SZL01-MSG-RET
                                            N-SZL01-NM-TAB
                                            N-SZL01-SQLCODE
                                            N-SZL01-SQLERRMC
      *    --- EXECUTA CALL
           MOVE 'SZEMNL01'               TO W-CALL

           EXEC SQL
             CALL SEGUROS.SZEMNL01
               ( :H-SZL01-COD-USUARIO
                 INDICATOR :N-SZL01-COD-USUARIO
               , :H-SZL01-COD-PROGRAMA
                 INDICATOR :N-SZL01-COD-PROGRAMA
               , :H-SZL01-DES-MSG-SISTEMA
                 INDICATOR :N-SZL01-DES-MSG-SISTEMA
               , :H-SZL01-IND-ERRO-LOG
                 INDICATOR :N-SZL01-IND-ERRO-LOG
               , :H-SZL01-SQLCODE-LOG
                 INDICATOR :N-SZL01-SQLCODE-LOG
               , :H-SZL01-SQLERRMC-LOG
                 INDICATOR :N-SZL01-SQLERRMC-LOG
               , :H-SZL01-DES-MSG-RETORNO
                 INDICATOR :N-SZL01-DES-MSG-RETORNO
               , :H-SZL01-SEQ-LOG-SISTEMA
                 INDICATOR :N-SZL01-SEQ-LOG-SISTEMA
               , :H-SZL01-IND-ERRO
                 INDICATOR :N-SZL01-IND-ERRO
               , :H-SZL01-MSG-RET
                 INDICATOR :N-SZL01-MSG-RET
               , :H-SZL01-NM-TAB
                 INDICATOR :N-SZL01-NM-TAB
               , :H-SZL01-SQLCODE
                 INDICATOR :N-SZL01-SQLCODE
               , :H-SZL01-SQLERRMC
                 INDICATOR :N-SZL01-SQLERRMC)
           END-EXEC

      *    --- VERIFICA SQL CALL FOI COM SUCESSO
           IF SQLCODE NOT = 000
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
      *       --- VERIFICA SE A PROCEDURE RETORNOU ERRO
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
           END-IF

           IF N-SZL01-SEQ-LOG-SISTEMA = -1
              INITIALIZE H-SZL01-SEQ-LOG-SISTEMA
           END-IF
           .
      *C0010-CALL-SP-SZEMNL01-EXIT. EXIT.

      *----------------------------------------------------------------*
       DB010-OPEN-CR-CERT-COMPRA.
      *----------------------------------------------------------------*
           MOVE 'DB010'                  TO W-LABEL

      *-----------------------------------------------------------------
      *--- PEGA TODOS CERTIFICADOS DO PRODUTO PROCESSADO
      *--- NOVOS OU PARCELAS PAGAS PARA COMPRA DA ASSISTENCIA SAF
      *-----------------------------------------------------------------
           EXEC SQL DECLARE CR_CERTIF_COMPRA CURSOR FOR
             SELECT VALUE(SZ012.NUM_CONTRATO_TERC,0) AS NUM_CONTR_TERC
                  , SZ012.DTA_INI_VIG_TERC + 1 MONTH AS DTA_PROX_COBRANC
                  , SZ011.COD_PRODUTO
                  , VALUE(SZ098.NUM_CONTRATO_FORN,' ') AS NUM_CONTR_FORN
                  , '0'                              AS NUM_VERSAO_SERIE
                  , VALUE(SZ098.COD_GCS_FORN,' ') AS COD_GCS_FORN
                  , SZ012.DTA_ASSINATURA
                  , VALUE(SZ012.DTA_FIM_VIG_TERC,'9999-12-31')
                                                  AS DTH_FIM_CONTRATO
                  , VALUE(SZ021.QTD_MESES_CONTRATO, 12)
                                                  AS QTD_MESES_CONTRATO
                  , SZ012.NUM_CONTRATO
                  , SZ010.NUM_APOLICE
                  , SZ011.NUM_ORI_CONTRATO
                  , SZ043.DTA_ACIONAMENTO         AS DTA_FIM_CARENCIA
                  , SZ012.NUM_PES_OPERADOR
                  , SZ012.NUM_LINHA_PRODUTO
                  , SZ012.NUM_CONTRATO_ANT
                  , SZ012.STA_CONTRATO_TERC
                  , SZ072.COD_ACOPLADO
                  , IFNULL(SZ098.COD_PROD_ACOPLADO, 0)
                  , IFNULL(SZ098.SEQ_ENVIO, 0) + 1   AS SEQ_ENVIO
                  , SZ115.SEQ_ACOPLADO
                  , SZ115.DTA_INI_VIGENCIA
                  , SZ115.DTA_FIM_VIGENCIA
                  , SZ043.IND_ENVIO
                  , SZ021.DTA_INI_VIG_SEG
                  , SZ021.DTA_FIM_VIG_SEG
             FROM SEGUROS.SZ_ORIGEM_CONTRATO SZ011
             JOIN SEGUROS.SZ_APOLICE         SZ010
               ON SZ010.SEQ_PROP_APOL      = SZ011.SEQ_APOLICE
             JOIN SEGUROS.SZ_CONTR_TERC      SZ012
               ON SZ012.NUM_ORI_CONTRATO   = SZ011.NUM_ORI_CONTRATO
              AND SZ012.STA_CONTRATO_TERC  = 'A'
             JOIN SEGUROS.SZ_CONTR_SEGURO    SZ021
               ON SZ021.NUM_CONTRATO       = SZ012.NUM_CONTRATO
             JOIN SEGUROS.SZ_APOL_ACOPLADO   SZ073
               ON SZ073.SEQ_APOLICE        = SZ011.SEQ_APOLICE
              AND (CURRENT DATE BETWEEN SZ073.DTA_INI_VIGENCIA
                   AND VALUE(SZ073.DTA_FIM_VIGENCIA,'2999-12-31'))
             JOIN SEGUROS.SZ_ACOPLADO        SZ072
               ON SZ072.COD_ACOPLADO       = SZ073.COD_ACOPLADO
              AND STA_ACOPLADO             = 'A'
              AND SZ072.COD_ACOPLADO       = 11
              AND SZ072.COD_TP_ACOPLADO    = 2
             JOIN SEGUROS.SZ_ACOPLADO_ASSIST SZ098
               ON SZ098.COD_ACOPLADO       = SZ072.COD_ACOPLADO
             JOIN SEGUROS.SZ_OBJ_ACOPLADO    SZ115
               ON SZ115.NUM_CONTRATO       = SZ012.NUM_CONTRATO
              AND SZ115.STA_ENVIO          = 'E'
              AND SZ115.COD_ACOPLADO       = SZ072.COD_ACOPLADO
              AND SZ115.DTA_VINCULACAO BETWEEN :WS-DT-INI
                                           AND :WS-DT-FIM
             JOIN SEGUROS.SZ_OBJ_ACOPLADO_ASSIST SZ043
               ON SZ043.NUM_CONTRATO       = SZ115.NUM_CONTRATO
              AND SZ043.COD_ACOPLADO       = SZ115.COD_ACOPLADO
              AND SZ043.SEQ_ACOPLADO       = SZ115.SEQ_ACOPLADO
             WHERE SZ011.COD_PRODUTO       BETWEEN :H-COD-PRODUTO-MIN
                                               AND :H-COD-PRODUTO-MAX
               AND SZ072.COD_ACOPLADO      BETWEEN :H-COD-ACOPLADO-MIN
                                               AND :H-COD-ACOPLADO-MAX
               AND CURRENT_DATE BETWEEN SZ012.DTA_INI_VIG_TERC
                                    AND SZ012.DTA_FIM_VIG_TERC
             ORDER BY SZ012.NUM_CONTRATO, SZ012.DTA_INI_VIG_TERC
             WITH UR
           END-EXEC.

           DISPLAY ' '
           DISPLAY W-PROGRAMA '- VAI ABRIR CURSOR CR_CERTIF_COMPRA COM:'
                               ' TIME=' FUNCTION CURRENT-DATE
           DISPLAY ' COD-PRODUTO INICIO  = ' H-COD-PRODUTO-MIN
           DISPLAY ' COD-PRODUTO FIM     = ' H-COD-PRODUTO-MAX
           DISPLAY ' COD-ACOPLADO INICIO = ' H-COD-ACOPLADO-MIN
           DISPLAY ' COD-ACOPLADO FIM    = ' H-COD-ACOPLADO-MAX
           DISPLAY ' '
           EXEC SQL
              OPEN  CR_CERTIF_COMPRA
           END-EXEC
           DISPLAY ' '
           DISPLAY W-PROGRAMA '-ABRIU     CURSOR CR_CERTIF_COMPRA AS '
                   FUNCTION CURRENT-DATE
           IF SQLCODE NOT EQUAL ZEROS
              INITIALIZE W-MENSAGEM-ERRO
              STRING 'OPEN CURSOR CR_CERTIF_COMPRA.'
                     ' COD-PRODUTO  BETWEEN '          E-INTEGER-1
                                      ' AND '          E-INTEGER-2
                     ' COD-ACOPLADO BETWEEN '          E-INTEGER-3
                                      ' AND '          E-INTEGER-4
                     DELIMITED BY SIZE INTO W-MENSAGEM-ERRO
              GO TO P9990-DB2-ERROR
           END-IF
           .
      *DB010-OPEN-CR-CERT-COMPRA.-EXIT. EXIT.

      *----------------------------------------------------------------*
       DB020-FETCH-CERT-COMPRA.
      *----------------------------------------------------------------*
           MOVE 'DB020'                  TO W-LABEL
           INITIALIZE SZ012-NUM-CONTRATO-TERC
                      SZ098-SEQ-ENVIO
                      SZ012-NUM-CONTRATO-ANT
           MOVE -1                       TO N-SZ012-NUM-CONTRATO-ANT
           EXEC SQL
              FETCH CR_CERTIF_COMPRA
                INTO   :SZ012-NUM-CONTRATO-TERC
                      ,:SZ012-DTA-INI-VIG-TERC
                      ,:SZ011-COD-PRODUTO
                      ,:SZ098-NUM-CONTRATO-FORN
                      ,:H-NUM-VERSAO-SERIE
                      ,:SZ098-COD-GCS-FORN
                      ,:SZ012-DTA-ASSINATURA
                      ,:H-DTA-INI-VIGENCIA-ATUAL
                      ,:SZ021-QTD-MESES-CONTRATO
                      ,:SZ012-NUM-CONTRATO
                      ,:SZ010-NUM-APOLICE
                      ,:SZ011-NUM-ORI-CONTRATO
                      ,:SZ043-DTA-ACIONAMENTO
                      ,:SZ012-NUM-PES-OPERADOR
                      ,:SZ012-NUM-LINHA-PRODUTO
                      ,:SZ012-NUM-CONTRATO-ANT
                       INDICATOR :N-SZ012-NUM-CONTRATO-ANT
                      ,:SZ012-STA-CONTRATO-TERC
                      ,:SZ072-COD-ACOPLADO
                      ,:SZ098-COD-PROD-ACOPLADO
                      ,:SZ098-SEQ-ENVIO
                      ,:SZ115-SEQ-ACOPLADO
                      ,:SZ115-DTA-INI-VIGENCIA
                      ,:SZ115-DTA-FIM-VIGENCIA
                      ,:SZ043-IND-ENVIO
                      ,:SZ021-DTA-INI-VIG-SEG
                      ,:SZ021-DTA-FIM-VIG-SEG
           END-EXEC

           EVALUATE SQLCODE
           WHEN +100
             MOVE 'SIM'                  TO W-FIM-CERTIF
             EXEC SQL CLOSE CR_CERTIF_COMPRA END-EXEC
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
           END-EVALUATE
           .
      *DB020-FETCH-CERT-COMPRA-EXIT. EXIT.

      *----------------------------------------------------------------*
       DB030-ABRIR-PACTUANTE.
      *----------------------------------------------------------------*
           MOVE 'DB030'                  TO W-LABEL
           MOVE SPACES                   TO W-FIM-PACTUANTE
           EXEC SQL
             OPEN CR_PACTUANTE
           END-EXEC
           IF SQLCODE NOT EQUAL ZEROS
              INITIALIZE W-MENSAGEM-ERRO
              STRING 'OPEN CURSOR CR_PACTUANTE.'
                     ' CONTR TERC='   W-NUM-CONTRATO-TERC
                     DELIMITED BY SIZE INTO W-MENSAGEM-ERRO
              GO TO P9990-DB2-ERROR
           END-IF
           .
      *DB030-ABRIR-PACTUANTE-EXIT. EXIT.

      *----------------------------------------------------------------*
       DB040-LER-PACTUANTE.
      *----------------------------------------------------------------*
           MOVE 'DB040'                  TO W-LABEL
           EXEC SQL
              FETCH CR_PACTUANTE
               INTO :SZ008-NOM-RAZ-SOCIAL
                  , :SZ008-NUM-CPF-CNPJ
                  , :SZ053-DTA-NASCIMENTO
                  , :SZ008-NUM-PESSOA
                  , :SZ053-IND-SEXO
                  , :SZ053-IND-ESTADO-CIVIL
                  , :SZ053-NUM-OCUPACAO
           END-EXEC
           EVALUATE SQLCODE
           WHEN 000
              ADD 1                      TO W-TOT-LIDOS-CERT-PACT
              MOVE SZ008-NUM-PESSOA      TO W-COD-PESSOA
           WHEN + 100
              MOVE 'SIM'                 TO W-FIM-PACTUANTE
              EXEC SQL CLOSE CR_PACTUANTE END-EXEC
           WHEN OTHER
             INITIALIZE W-MENSAGEM-ERRO
             STRING 'FETCH CURSOR CR_PACTUANTE.'
                    ' CONTR TERC='   W-NUM-CONTRATO-TERC
                     DELIMITED BY SIZE INTO W-MENSAGEM-ERRO
             GO TO P9990-DB2-ERROR
           END-EVALUATE
           .
      *DB040-LER-PACTUANTE-EXIT. EXIT.

      *----------------------------------------------------------------*
       DB050-LER-ENDERECO.
      *----------------------------------------------------------------*
           MOVE 'DB050'                  TO W-LABEL
           EXEC  SQL
                SELECT STRIP(NOM_LOGRADOURO)
                     , CASE WHEN NUM_LOGRADOURO IS NULL
                              OR NUM_LOGRADOURO = ''   THEN 'NF'
                            WHEN ASCII(SUBSTR(NUM_LOGRADOURO,1,1))
                               NOT BETWEEN 48 AND 57 THEN NUM_LOGRADOURO
                            ELSE LPAD(TRIM(NUM_LOGRADOURO),6,'0')
                       END AS NUM_END
                     , VALUE(DES_COMPL_ENDERECO, ' ')
                     , VALUE(NOM_BAIRRO, VALUE(DES_COMPL_ENDERECO, ' '))
                     , RTRIM(NOM_CIDADE) || '/' || VALUE(COD_UF, ' ')
                     , VALUE(COD_CEP,0)
                  INTO :SZ063-NOM-LOGRADOURO
                     , :SZ063-NUM-LOGRADOURO
                     , :SZ063-DES-COMPL-ENDERECO
                     , :SZ063-NOM-BAIRRO
                     , :SZ063-NOM-CIDADE
                     , :SZ063-COD-CEP
                  FROM SEGUROS.SZ_ENDERECO A
                 WHERE NUM_ENDERECO IN
                     ( SELECT NUM_ENDERECO
                         FROM SEGUROS.SZ_OBJ_ENDERECO
                        WHERE NUM_CONTRATO    = :SZ012-NUM-CONTRATO
                          AND STA_ENDERECO    = 'A'
                          AND COD_TP_ENDERECO IN('1','8','9')
                     )
                 FETCH FIRST 1 ROW ONLY
                  WITH UR
           END-EXEC

           IF SQLCODE = 100
              PERFORM DB051-LER-ENDERECO2
           END-IF

           IF SQLCODE NOT EQUAL ZEROS AND 100
              MOVE SZ012-NUM-CONTRATO    TO E-INTEGER-1
              INITIALIZE W-MENSAGEM-ERRO
              STRING 'SELECT SZ_OBJ_ENDERECO.'
                     ' NUM_CONTRATO=' E-INTEGER-1
                     ' CONTR TERC='   W-NUM-CONTRATO-TERC
                     ' CONTR SEG='    W-NUM-CONTRATO
                     ' NUM_PESSOA='   W-COD-PESSOA
                    DELIMITED BY SIZE INTO W-MENSAGEM-ERRO
              GO TO P9990-DB2-ERROR
           END-IF
           .
      *DB050-LER-ENDERECO-EXIT. EXIT.

      *----------------------------------------------------------------*
       DB051-LER-ENDERECO2.
      *----------------------------------------------------------------*
           MOVE 'DB051'                  TO W-LABEL
           EXEC  SQL
                SELECT STRIP(NOM_LOGRADOURO)
                     , CASE WHEN NUM_LOGRADOURO IS NULL
                              OR NUM_LOGRADOURO = ''   THEN 'NF'
                            WHEN ASCII(SUBSTR(NUM_LOGRADOURO,1,1))
                               NOT BETWEEN 48 AND 57 THEN NUM_LOGRADOURO
                            ELSE LPAD(TRIM(NUM_LOGRADOURO),6,'0')
                       END AS NUM_END
                     , VALUE(DES_COMPL_ENDERECO, ' ')
                     , VALUE(NOM_BAIRRO, VALUE(DES_COMPL_ENDERECO, ' '))
                     , RTRIM(NOM_CIDADE) || '/' || VALUE(COD_UF, ' ')
                     , VALUE(COD_CEP,0)
                  INTO :SZ063-NOM-LOGRADOURO
                     , :SZ063-NUM-LOGRADOURO
                     , :SZ063-DES-COMPL-ENDERECO
                     , :SZ063-NOM-BAIRRO
                     , :SZ063-NOM-CIDADE
                     , :SZ063-COD-CEP
                  FROM SEGUROS.SZ_ENDERECO A
                 WHERE NUM_ENDERECO IN
                     ( SELECT NUM_ENDERECO
                         FROM SEGUROS.SZ_OBJ_ENDERECO
                        WHERE NUM_CONTRATO    = :SZ012-NUM-CONTRATO
                          AND STA_ENDERECO    = 'A'
                     )
                 FETCH FIRST 1 ROW ONLY
                  WITH UR
           END-EXEC

           IF SQLCODE NOT EQUAL ZEROS
              MOVE SZ012-NUM-CONTRATO    TO E-INTEGER-1
              INITIALIZE W-MENSAGEM-ERRO
              STRING 'SELECT SZ_OBJ_ENDERECO2.'
                     ' NUM_CONTRATO=' E-INTEGER-1
                     ' CONTR TERC='   W-NUM-CONTRATO-TERC
                     ' CONTR SEG='    W-NUM-CONTRATO
                     ' NUM_PESSOA='   W-COD-PESSOA
                    DELIMITED BY SIZE INTO W-MENSAGEM-ERRO
              GO TO P9990-DB2-ERROR
           END-IF
           .
      *DB051-LER-ENDERECO2-EXIT. EXIT.

      *----------------------------------------------------------------*
       DB060-ACESSA-VIGENCIA-CONTR.
      *----------------------------------------------------------------*
           MOVE 'DB060'                  TO W-LABEL
           INITIALIZE H-DTA-INI-VIGENCIA-ORIG
                      H-DTA-FIM-CARENCIA-ORIG
           EXEC SQL
             SELECT SZ012.DTA_ASSINATURA
                    + :SZ021-QTD-MESES-CONTRATO months
                                               AS DTA_FIM_CARENCIA
                  , SZ012.DTA_FIM_VIG_TERC     AS DTA_FIM_VIGENCIA
                    INTO :H-DTA-INI-VIGENCIA-ORIG
                       , :H-DTA-FIM-CARENCIA-ORIG
             FROM SEGUROS.SZ_CONTR_TERC SZ012
             WHERE NUM_PES_OPERADOR          = :SZ012-NUM-PES-OPERADOR
               AND NUM_LINHA_PRODUTO         = :SZ012-NUM-LINHA-PRODUTO
               AND NUM_CONTRATO_TERC         = :SZ012-NUM-CONTRATO-ANT
             WITH UR
           END-EXEC

           IF SZEMB188-TRACE-ON-88
              MOVE SQLCODE               TO E-SQLCODE
              DISPLAY 'DB060-ACESSA-VIGENCIA-CONTR:'
              ' SQLCODE='                    E-SQLCODE
              ' SZ012-NUM-PES-OPERADOR='     SZ012-NUM-PES-OPERADOR
              ' SZ012-NUM-LINHA-PRODUTO='    SZ012-NUM-LINHA-PRODUTO
              ' SZ012-NUM-CONTRATO-ANT='     SZ012-NUM-CONTRATO-ANT
              ' H-DTA-INI-VIGENCIA-ORIG='    H-DTA-INI-VIGENCIA-ORIG
              ' H-DTA-FIM-CARENCIA-ORIG='    H-DTA-FIM-CARENCIA-ORIG
              ' TIME=' FUNCTION CURRENT-DATE
           END-IF

           IF SQLCODE NOT = 000
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
           END-IF
           .
      *DB060-ACESSA-VIGENCIA-CONTR-EXIT. EXIT.

      *----------------------------------------------------------------*
       DB070-LER-TELEFONE.
      *----------------------------------------------------------------*
           MOVE 'DB070'                  TO W-LABEL
           MOVE 55                       TO  W-DDI
           EXEC SQL
                SELECT SZ057.NUM_DDD
                     , SZ057.NUM_TELEFONE
                  INTO :SZ057-NUM-DDD
                     , :SZ057-NUM-TELEFONE
                  FROM SEGUROS.SZ_PESSOA_TELEFONE SZ057
                 WHERE NUM_PESSOA = :SZ008-NUM-PESSOA
                   AND SZ057.SEQ_TELEFONE =
                     ( SELECT MAX(SZ0571.SEQ_TELEFONE)
                        FROM SEGUROS.SZ_PESSOA_TELEFONE SZ0571
                       WHERE SZ0571.NUM_PESSOA = SZ057.NUM_PESSOA
                     )
                  WITH UR
           END-EXEC
           IF SQLCODE  EQUAL 100
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
           END-IF
           .
      *DB070-LER-TELEFONE-EXIT. EXIT.

      *----------------------------------------------------------------*
       DB080-DESC-PROFISSAO.
      *----------------------------------------------------------------*
           MOVE 'DB080'                  TO W-LABEL

           EXEC  SQL
                 SELECT DESCR_CBO
                     INTO :CBO-DESCR-CBO
                 FROM  SEGUROS.CBO
                 WHERE COD_CBO     = :SZ053-NUM-OCUPACAO
                 WITH UR
           END-EXEC
           IF SQLCODE  EQUAL 100
              MOVE  SPACES               TO CBO-DESCR-CBO
           ELSE
            IF SQLCODE NOT EQUAL ZEROS
               MOVE SZ053-NUM-OCUPACAO   TO W-NUM-OCUPACAO
               MOVE SZ072-COD-ACOPLADO   TO E-INTEGER-1
               MOVE SZ011-COD-PRODUTO    TO E-INTEGER-2
               INITIALIZE W-MENSAGEM-ERRO
               STRING 'SELECT SEGUROS.CBO. '
                      ' COD_CBO='      W-NUM-OCUPACAO
                      ' COD-ACOPLADO=' E-INTEGER-1
                      ' COD-PRODUTO= ' E-INTEGER-2
                     DELIMITED BY SIZE INTO W-MENSAGEM-ERRO
              GO TO P9990-DB2-ERROR
            END-IF
           END-IF
           .
      *DB080-DESC-PROFISSAO-EXIT. EXIT.

      *----------------------------------------------------------------
       P9000-FINALIZA.
      *----------------------------------------------------------------
      *    --- ROTINA DE FINALIZACAO E ESTATISTICA DO PROGRAMA
           MOVE 'P9000'                  TO W-LABEL
           .
      *P9000-FINALIZA-EXIT. EXIT.

      *----------------------------------------------------------------*
       DB900-EXECUTA-COMMIT.
      *----------------------------------------------------------------*
           EXEC SQL COMMIT END-EXEC

           IF SZEMB188-TRACE-ON-88
              DISPLAY W-PROGRAMA '- EXECUTOU COMMIT - '
                      FUNCTION CURRENT-DATE
           END-IF
           .
      *DB900-EXECUTA-COMMIT-EXIT. EXIT.

      *----------------------------------------------------------------*
       DB905-EXECUTA-ROLLBACK.
      *----------------------------------------------------------------*
           EXEC SQL ROLLBACK END-EXEC

           DISPLAY ' '
           DISPLAY '$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$'
                   '$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$'
           DISPLAY W-PROGRAMA
                   '- EXECUTOU ROLLBACK'
           DISPLAY '$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$'
                   '$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$'
           .
      *DB905-EXECUTA-ROLLBACK-EXIT. EXIT.

      *-----------------------------------------------------------------
      * PMONPR01 - BOOK COM OS PARAGRAFOS PARA PROCESSAR A MONITORACAO
      *            DE ARQUIVOS
      *-----------------------------------------------------------------
       COPY PMONPR01 REPLACING  ==:GE3000B:==  BY  ==GE3000B==.

      *----------------------------------------------------------------
      * P9990 - ROTINA ERRO DE DB2
      *----------------------------------------------------------------
       P9990-DB2-ERROR.
           MOVE  99                      TO  W-RETURN-CODE

           DISPLAY ' '
           DISPLAY '################################################'
                   '################################################'
           DISPLAY W-PROGRAMA ' - ERRO DE ACESSO AO BANCO DE DADOS DB2'
           DISPLAY '################################################'
                   '################################################'
           DISPLAY 'PARAGRAFO DE ORIGEM=' W-LABEL
           MOVE SQLCODE                  TO E-SQLCODE
           DISPLAY 'SQLCODE            =' E-SQLCODE
           MOVE SQLERRD(3)               TO E-SQLCODE
           DISPLAY 'SQLERRD(3)         =' E-SQLCODE
           IF SQLERRMC NOT = SPACES
              DISPLAY 'SQLERRMC=<'   SQLERRMC '>'
           END-IF
           DISPLAY '------------------------------------------------'
                   '------------------------------------------------'
           DISPLAY 'MENSAGEM:'
           DISPLAY '------------------------------------------------'
             '------------------------------------------------------'
           DISPLAY '<' W-MENSAGEM-ERRO(001:100) '>'
           IF W-MENSAGEM-ERRO(101:100) NOT = SPACES
              DISPLAY '<' W-MENSAGEM-ERRO(101:100) '>'
           END-IF
           IF W-MENSAGEM-ERRO(201:100) NOT = SPACES
              DISPLAY '<' W-MENSAGEM-ERRO(201:100) '>'
           END-IF
           IF W-MENSAGEM-ERRO(301:100) NOT = SPACES
              DISPLAY '<' W-MENSAGEM-ERRO(301:100) '>'
           END-IF
           IF W-MENSAGEM-ERRO(401:100) NOT = SPACES
              DISPLAY '<' W-MENSAGEM-ERRO(401:100) '>'
           END-IF
           DISPLAY '################################################'
                   '################################################'

           PERFORM DB905-EXECUTA-ROLLBACK

      *    --- GRAVA LOG
           MOVE 2                        TO H-SZL01-IND-ERRO-LOG
           MOVE 0                        TO N-SZL01-IND-ERRO-LOG
           PERFORM C0010-CALL-SP-SZEMNL01

           GO TO P9999-FINALIZACAO
           .
      *P9990-DB2-ERROR-EXIT. EXIT.

      *----------------------------------------------------------------
      * P9994 - ROTINA DISPLAY DE ERRO
      *----------------------------------------------------------------
       P9994-FIM-ANORMAL.
           MOVE  99                      TO  W-RETURN-CODE

           DISPLAY ' '
           DISPLAY '################################################'
                   '################################################'
           DISPLAY W-PROGRAMA ' - PROCESSAMENTO COM ERRO'
           DISPLAY '################################################'
                   '################################################'
           DISPLAY 'PARAGRAFO DE ORIGEM=' W-LABEL
           DISPLAY '------------------------------------------------'
                   '------------------------------------------------'
           DISPLAY 'MENSAGEM:'
           DISPLAY '------------------------------------------------'
             '------------------------------------------------------'
           DISPLAY '<' W-MENSAGEM-ERRO(001:100) '>'
           IF W-MENSAGEM-ERRO(101:100) NOT = SPACES
              DISPLAY '<' W-MENSAGEM-ERRO(101:100) '>'
           END-IF
           IF W-MENSAGEM-ERRO(201:100) NOT = SPACES
              DISPLAY '<' W-MENSAGEM-ERRO(201:100) '>'
           END-IF
           IF W-MENSAGEM-ERRO(301:100) NOT = SPACES
              DISPLAY '<' W-MENSAGEM-ERRO(301:100) '>'
           END-IF
           IF W-MENSAGEM-ERRO(401:100) NOT = SPACES
              DISPLAY '<' W-MENSAGEM-ERRO(401:100) '>'
           END-IF
           DISPLAY '################################################'
                   '################################################'

           PERFORM DB905-EXECUTA-ROLLBACK

      *    --- GRAVA LOG
           MOVE 1                        TO H-SZL01-IND-ERRO-LOG
           MOVE 0                        TO N-SZL01-IND-ERRO-LOG
           PERFORM C0010-CALL-SP-SZEMNL01

           GO TO P9999-FINALIZACAO
           .
      *P9994-FIM-ANORMAL-EXIT. EXIT.

      *----------------------------------------------------------------
       P9999-FINALIZACAO.
      *----------------------------------------------------------------
           IF W-OPEN-ARQ = 'SIM'
              MOVE 'NAO'                 TO W-OPEN-ARQ
              CLOSE ARQTEMP1
           END-IF

           DISPLAY ' '
           DISPLAY '---------------------------------------------------'
                   '---------------------------------------------------'
           DISPLAY W-PROGRAMA ' - TOTAIS DO PROCESSAMENTO'
           DISPLAY '---------------------------------------------------'
                   '---------------------------------------------------'
           DISPLAY 'QTD CERTIFICADOS LIDOS COMPRA   = '
                                                   W-TOT-LIDOS-CERT-COMP
           DISPLAY 'QTD CERTIFICADOS LIDOS PACTUANTE= '
                                                   W-TOT-LIDOS-CERT-PACT
           DISPLAY 'QTD COMPRA SAF/CESTA            = ' W-QTD-COMPRA1
           DISPLAY 'QTD RE-COMPRA SAF/CESTA         = ' W-QTD-COMPRA2
           DISPLAY 'QTD INCLUIDO OBJ_ACOPLADO       = ' W-ATUAL-OBJ-ACOP
           DISPLAY 'QTD INCLUIDO OBJ_ACOPL_ASSIST   = '
                                                   W-ATUAL-OBJ-ACOP-ASS
           DISPLAY 'QTD REGISTROS GRAVADOS ARQUIVO  = ' W-TOT-GRAVADOS
           DISPLAY '---------------------------------------------------'
                   '---------------------------------------------------'
           DISPLAY ' '

           IF W-RETURN-CODE = 0
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
           END-IF
           DISPLAY ' '

      *    --- ATUALIZA MONITOR ----------------------------------------
           PERFORM PMONITOR-ATUALIZA-MONITOR
      *    -------------------------------------------------------------

      *    ----FINALIZA-MONITORACAO ------------------------------------
           MOVE W-RETURN-CODE(2:5)       TO LK-GE3000B-COD-PROCESSAMENTO
           MOVE H-SZL01-SEQ-LOG-SISTEMA  TO LK-GE3000B-SEQ-LOG-SISTEMA
           PERFORM PMONITOR-FINALIZACAO
           PERFORM DB900-EXECUTA-COMMIT
      *    -------------------------------------------------------------

      *    --- ENCERRA PROGRAMA
           MOVE W-RETURN-CODE            TO RETURN-CODE
           STOP RUN
           .
      *P9999-FINALIZACAO-EXIT. EXIT.

     ************************** FIM SZEMB188 ***************************