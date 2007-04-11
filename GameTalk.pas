unit GameTalk;

interface

uses
  SysUtils, Windows, Messages, Classes, Graphics, Controls,
  Forms, Dialogs, Contnrs;

type
  IInterface = interface
  end;

  TPacket = class (TObject)
  public
    function ReadDate: TDateTime;
    function ReadFloat: Double;
    function ReadInteger: Integer;
    function ReadPacket: TPacket;
    function ReadString: string;
    procedure WriteDate(const Value: TDateTime);
    procedure WriteFloat(const Value: Double);
    procedure WriteInteger(const Value: Integer);
    procedure WritePacket(Data: TPacket);
    procedure WriteString(const Value: String);
  end;
  
  TDataAge = class (TObject)
  private
    FAge: TDateTime;
    FFieldID: Integer;
  public
    constructor Create(AFieldID: Integer);
    property Age: TDateTime read FAge;
    property FieldID: Integer read FFieldID;
  end;
  
  TDataAgeList = class (TObjectList)
  protected
    function GetItemCount: Integer;
    function GetItems(Index: Integer): TDataAge;
  public
    procedure Add(FieldID: Integer; Value: TObject);
    property ItemCount: Integer read GetItemCount;
    property Items[Index: Integer]: TDataAge read GetItems; default;
  end;
  
  TChangeLog = class (TObject)
  private
    FFieldID: Integer;
    FValue: TObject;
  public
    constructor Create(AFieldID: Integer; AValue: TObject);
    property FieldID: Integer read FFieldID;
    property Value: TObject read FValue;
  end;
  
  TChangeLogList = class (TObjectList)
  protected
    function GetItemCount: Integer;
    function GetItems(Index: Integer): TChangeLog;
  public
    procedure Add(FieldID: Integer; Value: TObject);
    property ItemCount: Integer read GetItemCount;
    property Items[Index: Integer]: TChangeLog read GetItems; default;
  end;
  
  IEntitySvr = interface (IUnknown)
    procedure Changed(FieldID: Integer; Value: TObject);
    function GetChangeLog: TChangeLogList;
    function Serialise: TPacket;
    property ChangeLog: TChangeLogList read GetChangeLog;
  end;
  
  IEntityCln = interface (IUnknown)
    procedure Deserialise(Data: TPacket);
  end;
  
  TEntity = class (TInterfacedObject)
  private
    FChangeLog: TChangeLogList;
    FDataAge: TDataAgeList;
    FID: Integer;
  protected
    function GetChangeLog: TChangeLogList;
  public
    constructor Create;
    destructor Destroy; override;
    property ChangeLog: TChangeLogList read GetChangeLog;
    property DataAge: TDataAgeList read FDataAge write FDataAge;
    property ID: Integer read FID write FID;
  end;
  
  TEntityList = class (TEntity)
  private
    FItems: TObjectList;
    function GetCount: Integer;
    function GetItems(Index: Integer): TEntity;
  public
    constructor Create;
    destructor Destroy; override;
    procedure Add(Entity: TEntity);
    property Count: Integer read GetCount;
    property Items[Index: Integer]: TEntity read GetItems; default;
  end;
  
  TEntityListSvr = class (TEntityList, IEntitySvr)
  public
    procedure Changed(FieldID: Integer; Value: TObject); virtual; abstract;
    function Serialise: TPacket; virtual; abstract;
  end;
  
  TEntityListCln = class (TEntityList, IEntityCln)
  public
    procedure Deserialise(Data: TPacket); virtual; abstract;
  end;
  

implementation

{ TPacket }

function TPacket.ReadDate: TDateTime;
begin
end;

function TPacket.ReadFloat: Double;
begin
end;

function TPacket.ReadInteger: Integer;
begin
end;

function TPacket.ReadPacket: TPacket;
begin
end;

function TPacket.ReadString: string;
begin
end;

procedure TPacket.WriteDate(const Value: TDateTime);
begin
end;

procedure TPacket.WriteFloat(const Value: Double);
begin
end;

procedure TPacket.WriteInteger(const Value: Integer);
begin
end;

procedure TPacket.WritePacket(Data: TPacket);
begin
end;

procedure TPacket.WriteString(const Value: String);
begin
end;

{ TDataAge }

constructor TDataAge.Create(AFieldID: Integer);
begin
  FAge := Now;
  FFieldID := AFieldID;
end;

{ TDataAgeList }

procedure TDataAgeList.Add(FieldID: Integer; Value: TObject);
begin
  inherited Add(TDataAge.Create(FieldID));
end;

function TDataAgeList.GetItemCount: Integer;
begin
  Result := Self.Count;
end;

function TDataAgeList.GetItems(Index: Integer): TDataAge;
begin
  Result := TDataAge(Self[Index]);
end;

{ TChangeLog }

constructor TChangeLog.Create(AFieldID: Integer; AValue: TObject);
begin
  FFieldID := AFieldID;
  FValue := AValue;
end;

{ TChangeLogList }

procedure TChangeLogList.Add(FieldID: Integer; Value: TObject);
begin
  inherited Add(TChangeLog.Create(FieldID, Value));
end;

function TChangeLogList.GetItemCount: Integer;
begin
  Result := Self.Count;
end;

function TChangeLogList.GetItems(Index: Integer): TChangeLog;
begin
  Result := TChangeLog(Self[Index]);
end;

{ TEntity }

constructor TEntity.Create;
begin
  inherited Create;
  FChangeLog := TChangeLogList.Create;
  FDataAge := TDataAgeList.Create;
end;

destructor TEntity.Destroy;
begin
  FChangeLog.Free;
  FDataAge.Free;
  inherited Destroy;
end;

function TEntity.GetChangeLog: TChangeLogList;
begin
  Result := FChangeLog;
end;

{ TEntityList }

constructor TEntityList.Create;
begin
  inherited Create;
  FItems := TObjectList.Create;
end;

destructor TEntityList.Destroy;
begin
  FItems.Free;
  inherited Destroy;
end;

procedure TEntityList.Add(Entity: TEntity);
var
  Index: Integer;
begin
  Index := FItems.Add(Entity);
  Entity.ID := Index;
end;

function TEntityList.GetCount: Integer;
begin
  Result := FItems.Count;
end;

function TEntityList.GetItems(Index: Integer): TEntity;
begin
  Result := TEntity(Items[Index]);
end;


end.
