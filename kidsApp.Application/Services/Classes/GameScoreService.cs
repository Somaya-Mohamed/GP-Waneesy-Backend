using AutoMapper;
using kidsApp.Application.DTOs.GameScoreDTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;

public class GameScoreService : IGameScoreService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GameScoreService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GameScoreDTO>> GetAllAsync()
    {
        var data = await _unitOfWork.GameScores.GetWithDetailsAsync();
        return _mapper.Map<IEnumerable<GameScoreDTO>>(data);
    }

    public async Task<GameScoreDTO?> GetByIdAsync(int id)
    {
        var entity = (await _unitOfWork.GameScores.GetWithDetailsAsync())
            .FirstOrDefault(x => x.Id == id);

        if (entity == null) return null;

        return _mapper.Map<GameScoreDTO>(entity);
    }
    
    
    public async Task<GameScoreDTO> CreateAsync(GameScoreCreateDTO dto)
    {
        // 1️⃣ تحقق من وجود اللعبة
        var game = await _unitOfWork.Games.GetByIdAsync(dto.GameId);
        if (game == null)
            throw new Exception($"Game with Id {dto.GameId} not found");

        // 2️⃣ تحقق من وجود الطفل
        var child = await _unitOfWork.Children.GetByIdAsync(dto.ChildId);
        if (child == null)
            throw new Exception($"Child with Id {dto.ChildId} not found");

        // 3️⃣ إنشاء كائن GameScore
        var entity = _mapper.Map<GameScore>(dto);
        entity.Date = DateTime.UtcNow;

        // 4️⃣ اربط الـ navigation properties
        entity.Game = game;
        entity.Child = child;

        // 5️⃣ أضف الكائن إلى الـ DbSet
        await _unitOfWork.GameScores.AddAsync(entity);

        // 6️⃣ احفظ التغييرات في قاعدة البيانات
        await _unitOfWork.SaveChangesAsync();

        // 7️⃣ إعادة تحميل الكائن مع التفاصيل (navigation properties)
        var savedEntity = (await _unitOfWork.GameScores.GetWithDetailsAsync())
            .First(x => x.Id == entity.Id);

        // 8️⃣ حوله إلى DTO وأرجعه
        return _mapper.Map<GameScoreDTO>(savedEntity);
    }
    //public async Task<GameScoreDTO> CreateAsync(GameScoreCreateDTO dto)
    //{
    //    // جلب Game و Child من DB
    //    var game = await _unitOfWork.Games.GetByIdAsync(dto.GameId);
    //    if (game == null)
    //        throw new Exception($"Game with Id {dto.GameId} not found");

    //    var child = await _unitOfWork.Children.GetByIdAsync(dto.ChildId);
    //    if (child == null)
    //        throw new Exception($"Child with Id {dto.ChildId} not found");

    //    // عمل Mapping من DTO ل Entity
    //    var entity = _mapper.Map<GameScore>(dto);
    //    entity.Date = DateTime.UtcNow;

    //    // ربط Navigation properties
    //    entity.Game = game;
    //    entity.Child = child;

    //    // إضافة للحفظ
    //    await _unitOfWork.GameScores.AddAsync(entity);
    //    await _unitOfWork.SaveChangesAsync();

    //    // إعادة التحميل مع Include لتجنب null في ChildName و GameTitle
    //    var savedEntity = (await _unitOfWork.GameScores.GetWithDetailsAsync())
    //        .First(x => x.Id == entity.Id);

    //    return _mapper.Map<GameScoreDTO>(savedEntity);
    //}




    //public async Task<GameScoreDTO> CreateAsync(GameScoreCreateDTO dto)
    //{
    //    var entity = _mapper.Map<GameScore>(dto);
    //    entity.Date = DateTime.UtcNow;

    //    await _unitOfWork.GameScores.AddAsync(entity);
    //    await _unitOfWork.SaveChangesAsync();

    //    return _mapper.Map<GameScoreDTO>(entity);
    //}

    public async Task<bool> UpdateAsync(int id, GameScoreUpdateDTO dto)
    {
        var entity = await _unitOfWork.GameScores.GetByIdAsync(id);
        if (entity == null) return false;

        _mapper.Map(dto, entity);
        _unitOfWork.GameScores.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _unitOfWork.GameScores.GetByIdAsync(id);
        if (entity == null) return false;

        _unitOfWork.GameScores.Delete(entity);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<GameScoreDTO>> GetScoresByGameIdAsync(int gameId)
    {
        var scores = (await _unitOfWork.GameScores.GetWithDetailsAsync())
            .Where(x => x.GameId == gameId);

        return _mapper.Map<IEnumerable<GameScoreDTO>>(scores);
    }

    public async Task<IEnumerable<GameScoreDTO>> GetTopScoresAsync(int topCount)
    {
        var scores = (await _unitOfWork.GameScores.GetWithDetailsAsync())
            .OrderByDescending(x => x.ScoreValue)
            .Take(topCount);

        return _mapper.Map<IEnumerable<GameScoreDTO>>(scores);
    }
}