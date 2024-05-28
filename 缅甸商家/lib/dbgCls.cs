//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static System.Runtime.InteropServices.JavaScript.JSType;

//namespace 缅甸商家.lib
//{
//    internal class dbgCls
//    {

//$GLOBALS['dbg']=[];

//$GLOBALS['dbg_show']=true;
///**
// * @param string $METHOD__
// * @return void
// */
//function setDbgFunEnter(string $METHOD__,$func_get_args): void
//{
//    if($GLOBALS['dbg_show']==false)
//        return;
//    $GLOBALS['dbgpad']=$GLOBALS['dbgpad']+4;

//    $logmsg = str_repeat(" ", $GLOBALS['dbgpad']) . $METHOD__ . '(((' . json_encode($func_get_args) . ")))";
//    array_push($GLOBALS['dbg'],$logmsg   );
//        print_r($logmsg."\n");
//}


//function setDbgVal(string $METHOD__,  $vname,$val): void
//{
//    if($GLOBALS['dbg_show']==false)
//        return;
//    $msg = str_repeat(" ", $GLOBALS['dbgpad']) . $METHOD__ . ":: $vname=>$val";
//    array_push($GLOBALS['dbg'],
//        $msg);
//    print_r($msg."\n");

//}


///**
// * @param string $METHOD__
// * @param float $retval
// * @return void
// */
//function setDbgRtVal(string $METHOD__,  $retval): void
//{
//    if($GLOBALS['dbg_show']==false)
//        return;
//    // ENDFUN
//    $msglog = str_repeat(" ", $GLOBALS['dbgpad']) . "" . $METHOD__ . ':: ret=>' . json_encode($retval) ;
//    print_r($msglog."\n");
//    array_push($GLOBALS['dbg'], $msglog);
//    $GLOBALS['dbgpad']=$GLOBALS['dbgpad']-4;
//}
