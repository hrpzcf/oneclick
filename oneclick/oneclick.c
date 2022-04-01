#include <shlwapi.h>
#include <windows.h>
#pragma comment(lib, "shlwapi.lib")

#define CLIP_FULLPATH L"-fullpath"
#define CLIP_BASENAME L"-basename"

int WINAPI wWinMain(_In_ HINSTANCE hInstance, _In_opt_ HINSTANCE hPrevInstance, _In_ LPWSTR lpCmdLine, _In_ int nShowCmd) {
    HGLOBAL GMEMClipbord = NULL;
    INT ArgCount;
    LPWSTR *ArgList, ModPath;
    SIZE_T szPath;
    UINT uType = MB_TOPMOST | MB_ICONWARNING;
    if (NULL == (ArgList = CommandLineToArgvW(lpCmdLine, &ArgCount)))
        return EXIT_FAILURE;
    if (ArgCount < 2) {
        MessageBoxW(NULL, L"常用参数：-fullpath \"%V\"或-basename \"%V\"", L"错误", uType);
        return EXIT_FAILURE;
    }
    if (wcscmp(ArgList[0], CLIP_FULLPATH) == 0)
        ModPath = ArgList[1];
    else if (wcscmp(ArgList[0], CLIP_BASENAME) == 0)
        ModPath = PathFindFileNameW(ArgList[1]);
    else {
        MessageBoxW(NULL, L"参数错误，常用：-fullpath \"%V\"或-basename \"%V\"", L"错误", uType);
        goto FreeAndReturn;
    }
    szPath = sizeof(WCHAR) * (wcslen(ModPath) + 1);
    GMEMClipbord = GlobalAlloc(GPTR, szPath);
    if (NULL == GMEMClipbord) goto FreeAndReturn;
    memcpy_s(GMEMClipbord, szPath, ModPath, szPath);
    OpenClipboard(NULL); EmptyClipboard();
    SetClipboardData(CF_UNICODETEXT, GMEMClipbord); CloseClipboard();
FreeAndReturn:
    LocalFree(ArgList);
    return EXIT_SUCCESS;
}
