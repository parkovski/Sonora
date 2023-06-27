#pragma once

struct HostInterface {
    typedef void (*FnHello)();

    FnHello Hello;
};

#if 0
struct HostedMainArgs {
    int argc;
    const char_t *const *argv;
    HostInterface host;
};
#endif
