using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BTTank : BehaviourTree
{
    Selector rootSelector;
    Sequence seeEnemySequence;
    SeeEnemyBT seeEnemy;

    //See Enemy
    Selector seeEnemySelector;
    Sequence seenByEnemySequence;
    SeenByEnemyBT seenByEnemy;
    //Seen by Enemy
    Selector seenByEnemySelector;
    Sequence seenHasAmmoSequence;
    HasAmmoBT seenHasAmmo;

    //Seen Has Ammo

    Selector hasAmmoSelector;
    Sequence healthLowSequence;
    IsHealthLowBT healthLow;

    //Health Low
    Selector lowHealthSelector;
    Sequence enemyLowHealthSequence;
    EnemyHealthLowBT enemyLowHealth;

    //EnemyHealthLow
    Selector enemyHealthLowSelector;
    Sequence enemyHealthLowRangeSequence;
    InRangeBT enemyHealthLowRange;
    Parallel enemyHealthLowParallel;
    Selector enemyHealthLowEvadeRetreatSelector;
    Sequence enemyHealthLowEvadeSequence;
    ShotAtBT enemyHealthLowEvade;
    EvadeBT enemyHealthLowEvadeEvade;
    Sequence enemyHealthLowRetreatSequence;
    TooCloseToEnemyBT enemyHealthLowRetreat;
    FleeBT enemyHealthLowRetreatRetreat;
    Sequence enemyHealthLowShootSequence;
    CanShootBT enemyHealthLowShoot;
    ShootBT enemyHealthLowShootShoot;
    MoveTowardsTargetBT enemyHealthLowMove;

    //EnemyHealthHigh
    Parallel enemyHealthHighParallel;
    Sequence enemyHealthHighEvadeSequence;
    ShotAtBT enemyHealthHighEvade;
    EvadeBT enemyHealthHighEvadeEvade;
    Selector enemyHealthHighFleeSelector;
    Sequence enemyHealthHighCoverSequence;
    SeeCoverBT enemyHealthHighCover;
    GetToCoverBT enemyHealthHighCoverCover;
    FleeBT enemyHealthHighFlee;


    //Health High
    Selector highHealthSelector;
    Sequence highHealthRangeSequence;
    InRangeBT highHealthRange;
    Parallel highHealthParallel;
    Selector highHealthEvadeRetreatSelector;
    Sequence highHealthEvadeSequence;
    ShotAtBT highHealthEvade;
    EvadeBT highHealthEvadeEvade;
    Sequence highHealthRetreatSequence;
    TooCloseToEnemyBT highHealthRetreat;
    FleeBT highHealthRetreatRetreat;
    Sequence highHealthShootSequence;
    CanShootBT highHealthShoot;
    ShootBT highHealthShootShoot;
    MoveTowardsTargetBT highHealthMove;
    
    //Seen No Ammo

    Parallel reloadFleeParallel;

    ReloadBT reloadFleeReload;

    Selector reloadFleeSelector;
    Sequence reloadCoverSequence;
    SeeCoverBT reloadCover;
    GetToCoverBT reloadCoverCover;
    FleeBT reloadFleeFlee;

    Sequence reloadEvadeSequence;
    ShotAtBT reloadEvade;
    EvadeBT reloadEvadeEvade;


    //Not Seen by enemy
    Selector notSeenSelector;
    Sequence notSeenHasAmmoSequence;
    HasAmmoBT notSeenHasAmmo;
    ReloadBT notSeenReload;
    Selector notSeenhasAmmoSelector;
    Sequence notSeenRangeSequence;
    InRangeBT notSeenRange;
    Sequence notSeenShootSequence;
    CanShootBT notSeenShoot;
    ShootBT notSeenShootShoot;
    MoveTowardsTargetBT notSeenMoveTowards;

    //If not see Enemy
    Selector notSeeEnemySelector;
    Sequence notSeeTimeSequence;
    TimeSincePositionKnownBT notSeeTime;
    Parallel notSeeFindParallel;
    MoveLastKnownPositionBT notSeeFind;
    Selector notSeeFindSelector;
    HasAmmoBT notSeeFindAmmo;
    ReloadBT notSeeFindReload;

    Parallel WanderReloadParalllel;
    WanderBT WanderReloadWander;
    Selector WanderReloadSelector;
    AmmoFullBT WanderReloadAmmo;
    ReloadBT WanderReloadReload;


    public BTTank(Agent ownerAgent) : base(ownerAgent)
    {
        
        rootSelector = new Selector(ownerAgent);
        //Check if can see enemy
        seeEnemySequence = new Sequence(ownerAgent);
        seeEnemy = new SeeEnemyBT(ownerAgent);

        //See Enemy
        seeEnemySelector = new Selector(ownerAgent);
        seenByEnemySequence = new Sequence(ownerAgent);
        seenByEnemy = new SeenByEnemyBT(ownerAgent);

        //SeenByEmemy
        seenByEnemySelector = new Selector(ownerAgent);
        seenHasAmmoSequence = new Sequence(ownerAgent);
        seenHasAmmo = new HasAmmoBT(ownerAgent);

        //SeenHasAmmo
        hasAmmoSelector = new Selector(ownerAgent);
        healthLowSequence = new Sequence(ownerAgent);
        healthLow = new IsHealthLowBT(ownerAgent);

        //Health Low
        lowHealthSelector = new Selector(ownerAgent);
        enemyLowHealthSequence = new Sequence(ownerAgent);
        enemyLowHealth = new EnemyHealthLowBT(ownerAgent);

        //Enemy Health Low
        enemyHealthLowSelector = new Selector(ownerAgent);
        enemyHealthLowRangeSequence = new Sequence(ownerAgent);
        enemyHealthLowRange = new InRangeBT(ownerAgent);
        enemyHealthLowParallel = new Parallel(ownerAgent);
        enemyHealthLowEvadeRetreatSelector = new Selector(ownerAgent);
        enemyHealthLowEvadeSequence = new Sequence(ownerAgent);
        enemyHealthLowEvade = new ShotAtBT(ownerAgent);
        enemyHealthLowEvadeEvade = new EvadeBT(ownerAgent);
        enemyHealthLowRetreatSequence = new Sequence(ownerAgent);
        enemyHealthLowRetreat = new TooCloseToEnemyBT(ownerAgent);
        enemyHealthLowRetreatRetreat = new FleeBT(ownerAgent);
        
        enemyHealthLowShootSequence = new Sequence(ownerAgent);
        enemyHealthLowShoot = new CanShootBT(ownerAgent);
        enemyHealthLowShootShoot = new ShootBT(ownerAgent);
        enemyHealthLowMove = new MoveTowardsTargetBT(ownerAgent);

        //Enemy Health High
        enemyHealthHighParallel = new Parallel(ownerAgent);
        enemyHealthHighEvadeSequence = new Sequence(ownerAgent);
        enemyHealthHighEvade = new ShotAtBT(ownerAgent);
        enemyHealthHighEvadeEvade = new EvadeBT(ownerAgent);
        enemyHealthHighFleeSelector = new Selector(ownerAgent);
        enemyHealthHighCoverSequence = new Sequence(ownerAgent);
        enemyHealthHighCover = new SeeCoverBT(ownerAgent);
        enemyHealthHighCoverCover = new GetToCoverBT(ownerAgent);
        enemyHealthHighFlee = new FleeBT(ownerAgent);

        //Health High
        highHealthSelector = new Selector(ownerAgent);
        highHealthRangeSequence = new Sequence(ownerAgent);
        highHealthRange = new InRangeBT(ownerAgent);
        highHealthParallel = new Parallel(ownerAgent);
        highHealthEvadeRetreatSelector = new Selector(ownerAgent);
        highHealthEvadeSequence = new Sequence(ownerAgent);
        highHealthEvade = new ShotAtBT(ownerAgent);
        highHealthEvadeEvade = new EvadeBT(ownerAgent);
        highHealthShootSequence = new Sequence(ownerAgent);
        highHealthRetreatSequence = new Sequence(ownerAgent);
        highHealthRetreat = new TooCloseToEnemyBT(ownerAgent);
        highHealthRetreatRetreat = new FleeBT(ownerAgent);
        highHealthShoot = new CanShootBT(ownerAgent);
        highHealthShootShoot = new ShootBT(ownerAgent);
        highHealthMove = new MoveTowardsTargetBT(ownerAgent);

        //SeenNoAmmo
        reloadFleeParallel = new Parallel(ownerAgent);

        reloadFleeReload = new ReloadBT(ownerAgent);

        reloadFleeSelector = new Selector(ownerAgent);
        reloadCoverSequence = new Sequence(ownerAgent);
        reloadCover = new SeeCoverBT(ownerAgent);
        reloadCoverCover = new GetToCoverBT(ownerAgent);
        reloadFleeFlee = new FleeBT(ownerAgent);

        reloadEvadeSequence = new Sequence(ownerAgent);
        reloadEvade = new ShotAtBT(ownerAgent);
        reloadEvadeEvade = new EvadeBT(ownerAgent);

        //NotSeenByEnemy
        notSeenSelector = new Selector(ownerAgent);
        notSeenHasAmmoSequence = new Sequence(ownerAgent);
        notSeenHasAmmo = new HasAmmoBT(ownerAgent);
        notSeenReload = new ReloadBT(ownerAgent);
        notSeenhasAmmoSelector = new Selector(ownerAgent);
        notSeenRangeSequence = new Sequence(ownerAgent);
        notSeenRange = new InRangeBT(ownerAgent);
        notSeenShootSequence = new Sequence(ownerAgent);
        notSeenShoot = new CanShootBT(ownerAgent);
        notSeenShootShoot = new ShootBT(ownerAgent);
        notSeenMoveTowards = new MoveTowardsTargetBT(ownerAgent);
        
        //NotSeeEnemy
        notSeeEnemySelector = new Selector(ownerAgent);
        notSeeTimeSequence = new Sequence(ownerAgent);
        notSeeTime = new TimeSincePositionKnownBT(ownerAgent);
        notSeeFindParallel = new Parallel(ownerAgent);
        notSeeFind = new MoveLastKnownPositionBT(ownerAgent);
        notSeeFindSelector = new Selector(ownerAgent);
        notSeeFindAmmo = new HasAmmoBT(ownerAgent);
        notSeeFindReload = new ReloadBT(ownerAgent);

        WanderReloadParalllel = new Parallel(ownerAgent);
        WanderReloadWander = new WanderBT(ownerAgent);
        WanderReloadSelector = new Selector(ownerAgent);
        WanderReloadAmmo = new AmmoFullBT(ownerAgent);
        WanderReloadReload = new ReloadBT(ownerAgent);


        rootNode = rootSelector;
        //Check if can see enemy
        rootSelector.AddChild(seeEnemySequence);
        seeEnemySequence.AddChild(seeEnemy);
        //Can See Enemy
        seeEnemySequence.AddChild(seeEnemySelector);
        seeEnemySelector.AddChild(seenByEnemySequence);
        seenByEnemySequence.AddChild(seenByEnemy);

        //Seen by enemy 
        seenByEnemySequence.AddChild(seenByEnemySelector);
        seenByEnemySelector.AddChild(seenHasAmmoSequence);
        seenHasAmmoSequence.AddChild(seenHasAmmo);

        //SeenHasAmmo

        seenHasAmmoSequence.AddChild(hasAmmoSelector);
        hasAmmoSelector.AddChild(healthLowSequence);
        healthLowSequence.AddChild(healthLow);

        //Health Low
        healthLowSequence.AddChild(lowHealthSelector);
        lowHealthSelector.AddChild(enemyLowHealthSequence);
        enemyLowHealthSequence.AddChild(enemyLowHealth);

        //Enemy Health Low
        enemyLowHealthSequence.AddChild(enemyHealthLowSelector);
        enemyHealthLowSelector.AddChild(enemyHealthLowRangeSequence);
        enemyHealthLowRangeSequence.AddChild(enemyHealthLowRange);
        enemyHealthLowRangeSequence.AddChild(enemyHealthLowParallel);
        enemyHealthLowParallel.AddChild(enemyHealthLowEvadeRetreatSelector);
        enemyHealthLowEvadeRetreatSelector.AddChild(enemyHealthLowEvadeSequence);
        enemyHealthLowEvadeSequence.AddChild(enemyHealthLowEvade);
        enemyHealthLowEvadeSequence.AddChild(enemyHealthLowEvadeEvade);
        enemyHealthLowEvadeRetreatSelector.AddChild(enemyHealthLowRetreatSequence);
        enemyHealthLowRetreatSequence.AddChild(enemyHealthLowRetreat);
        enemyHealthLowRetreatSequence.AddChild(enemyHealthLowRetreatRetreat);

        enemyHealthLowParallel.AddChild(enemyHealthLowShootSequence);
        enemyHealthLowShootSequence.AddChild(enemyHealthLowShoot);
        enemyHealthLowShootSequence.AddChild(enemyHealthLowShootShoot);
        enemyHealthLowSelector.AddChild(enemyHealthLowMove);

        //Enemy Health High
        lowHealthSelector.AddChild(enemyHealthHighParallel);
        enemyHealthHighParallel.AddChild(enemyHealthHighEvadeSequence);
        enemyHealthHighEvadeSequence.AddChild(enemyHealthHighEvade);
        enemyHealthHighEvadeSequence.AddChild(enemyHealthHighEvadeEvade);
        enemyHealthHighParallel.AddChild(enemyHealthHighFleeSelector);
        enemyHealthHighFleeSelector.AddChild(enemyHealthHighCoverSequence);
        enemyHealthHighCoverSequence.AddChild(enemyHealthHighCover);
        enemyHealthHighCoverSequence.AddChild(enemyHealthHighCoverCover);
        enemyHealthHighFleeSelector.AddChild(enemyHealthHighFlee);

        //Health High
        hasAmmoSelector.AddChild(highHealthSelector);
        highHealthSelector.AddChild(highHealthRangeSequence);
        highHealthRangeSequence.AddChild(highHealthRange);
        highHealthRangeSequence.AddChild(highHealthParallel);
        highHealthParallel.AddChild(highHealthMove);
        highHealthParallel.AddChild(highHealthEvadeRetreatSelector);
        highHealthEvadeRetreatSelector.AddChild(highHealthEvadeSequence);
        highHealthEvadeSequence.AddChild(highHealthEvade);
        highHealthEvadeSequence.AddChild(highHealthEvadeEvade);
        highHealthEvadeRetreatSelector.AddChild(highHealthRetreatSequence);
        highHealthRetreatSequence.AddChild(highHealthRetreat);
        highHealthRetreatSequence.AddChild(highHealthRetreatRetreat);

        highHealthParallel.AddChild(highHealthShootSequence);
        highHealthShootSequence.AddChild(highHealthShoot);
        highHealthShootSequence.AddChild(highHealthShootShoot);


        //Seen, not have ammo

        seenByEnemySelector.AddChild(reloadFleeParallel);
        reloadFleeParallel.AddChild(reloadFleeReload);
        reloadFleeParallel.AddChild(reloadFleeSelector);
        reloadFleeSelector.AddChild(reloadCoverSequence);
        reloadCoverSequence.AddChild(reloadCover);
        reloadCoverSequence.AddChild(reloadCoverCover);
        reloadFleeSelector.AddChild(reloadFleeFlee);
        reloadFleeParallel.AddChild(reloadEvadeSequence);
        reloadEvadeSequence.AddChild(reloadEvade);
        reloadCoverSequence.AddChild(reloadEvadeEvade);


        //NotSeenByEnemy
        seeEnemySelector.AddChild(notSeenSelector);
        notSeenSelector.AddChild(notSeenHasAmmoSequence);
        notSeenSelector.AddChild(notSeenReload);
        notSeenHasAmmoSequence.AddChild(notSeenHasAmmo);
        notSeenHasAmmoSequence.AddChild(notSeenhasAmmoSelector);
        notSeenhasAmmoSelector.AddChild(notSeenRangeSequence);
        notSeenhasAmmoSelector.AddChild(notSeenMoveTowards);
        notSeenRangeSequence.AddChild(notSeenRange);
        notSeenRangeSequence.AddChild(notSeenShootSequence);
        notSeenShootSequence.AddChild(notSeenShoot);
        notSeenShootSequence.AddChild(notSeenShootShoot);

        //Can't See Enemy
        rootSelector.AddChild(notSeeEnemySelector);
        notSeeEnemySelector.AddChild(notSeeTimeSequence);
        notSeeTimeSequence.AddChild(notSeeTime);
        notSeeTimeSequence.AddChild(notSeeFindParallel);
        notSeeFindParallel.AddChild(notSeeFind);
        notSeeFindParallel.AddChild(notSeeFindSelector);
        notSeeFindSelector.AddChild(notSeeFindAmmo);
        notSeeFindSelector.AddChild(notSeeFindReload);

        notSeeEnemySelector.AddChild(WanderReloadParalllel);
        WanderReloadParalllel.AddChild(WanderReloadWander);
        WanderReloadParalllel.AddChild(WanderReloadSelector);
        WanderReloadSelector.AddChild(WanderReloadAmmo);
        WanderReloadSelector.AddChild(WanderReloadReload);

    }
}
