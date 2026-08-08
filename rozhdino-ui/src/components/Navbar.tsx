import { useState, useEffect } from "react";
import { Menu, X } from "lucide-react";
import { motion } from "framer-motion";
import Container from "./ui/Container";
import { Link } from "react-router-dom";

export default function Navbar() {
  const [open, setOpen] = useState(false);
  const [hide, setHide] = useState(false);

  useEffect(() => {
    let lastScroll = 0;

    const handleScroll = () => {
      const currentScroll = window.scrollY;

      if (currentScroll > lastScroll && currentScroll > 100) {
        // scroll down
        setHide(true);
      } else {
        // scroll up
        setHide(false);
      }

      lastScroll = currentScroll;
    };

    window.addEventListener("scroll", handleScroll);

    return () => {
      window.removeEventListener("scroll", handleScroll);
    };
  }, []);

  return (
    <motion.nav
      animate={{
        y: hide ? "-100%" : 0,
      }}
      transition={{
        duration: 0.3,
      }}
      className="
            fixed
            top-0
            left-0
            w-full
            z-50
            bg-white/80
            backdrop-blur-md
            border-b
            border-purple-400
            "
    >
      <div
        className="
              absolute
              bottom-0
              left-0
              w-full
              h-[2px]
              bg-gradient-to-r
              from-purple-600
              to-green-400
              "
      ></div>

      <Container>
        <div
          className="
                    h-20
                    flex
                    items-center
                    justify-between
                    "
        >
          {/* Logo on right navbar*/}
          <div className="flex items-center">
            <Link
              to="/"
              className="
                group
                flex
                items-center
                gap-3
                transition-all
                duration-300
                "
            >
              {/* Logo */}
              {/*can be photo,span, etc ...*/}
              <div
                className="
                flex
                h-11
                w-11
                items-center
                justify-center
                rounded-2xl
                bg-gradient-to-br
                from-purple-600
                to-fuchsia-500
                text-white
                shadow-lg
                shadow-purple-500/30
                transition-all
                duration-300
                group-hover:scale-110
                group-hover:rotate-3
              "
              >
                <span className="text-xl font-black"> R </span>
              </div>

              {/* Brand Name */}
              <div className="flex flex-col leading-none">
                <span
                  className="
                  text-lg
                  font-black
                  tracking-tight
                  text-gray-900
                  transition-all
                  duration-300
                  group-hover:scale-110
                  group-hover:rotate-2
                  group-hover:text-purple-600
                "
                >
                  رشدینو
                </span>

                <span
                  className="
                     text-lg
                      font-black
                      tracking-tight
                      text-gray-900
                      transition-all
                      duration-300
                      group-hover:scale-110
                      group-hover:rotate-2
                      group-hover:text-purple-600
                "
                >
                  ROSHDINO
                </span>
              </div>
            </Link>
          </div>

          {/* Desktop Menu */}

          <nav
            className="
                        hidden
                        md:flex
                        items-center
                        gap-10
                        font-medium
                        text-gray-700
                        "
          >
            <Link
              to="/"
              className="
                      relative
                      text-gray-700
                      font-medium
                      transition-colors
                      duration-300
                      hover:text-purple-600
                      group
                  "
            >
              خانه
              <span
                className="
                          absolute
                          -bottom-2
                          right-0
                          h-[2px]
                          w-0
                          bg-purple-600
                          transition-all
                          duration-300
                          group-hover:w-full
                      "
              />
            </Link>

            <Link
              to="/services"
              className="
                      relative
                      text-gray-700
                      font-medium
                      transition-colors
                      duration-300
                      hover:text-purple-600
                      group
                  "
            >
              خدمات
              <span
                className="
                          absolute
                          -bottom-2
                          right-0
                          h-[2px]
                          w-0
                          bg-purple-600
                          transition-all
                          duration-300
                          group-hover:w-full
                      "
              />
            </Link>

            <Link
              to="/portfolio"
              className="
                      relative
                      text-gray-700
                      font-medium
                      transition-colors
                      duration-300
                      hover:text-purple-600
                      group
                  "
            >
              نمونه کارها
              <span
                className="
                          absolute
                          -bottom-2
                          right-0
                          h-[2px]
                          w-0
                          bg-purple-600
                          transition-all
                          duration-300
                          group-hover:w-full
                      "
              />
            </Link>

            <Link
              to="/about"
              className="
                      relative
                      text-gray-700
                      font-medium
                      transition-colors
                      duration-300
                      hover:text-purple-600
                      group
                  "
            >
              درباره ما
              <span
                className="
                          absolute
                          -bottom-2
                          right-0
                          h-[2px]
                          w-0
                          bg-purple-600
                          transition-all
                          duration-300
                          group-hover:w-full
                      "
              />
            </Link>

            <Link
              to="/contact"
              className="
                      relative
                      text-gray-700
                      font-medium
                      transition-colors
                      duration-300
                      hover:text-purple-600
                      group
                  "
            >
              تماس
              <span
                className="
                          absolute
                          -bottom-2
                          right-0
                          h-[2px]
                          w-0
                          bg-purple-600
                          transition-all
                          duration-300
                          group-hover:w-full
                      "
              />
            </Link>
          </nav>

          {/* Desktop Button */}

          <button
            name="startR"
            className="
                        hidden
                        md:block
                        bg-purple-600
                        text-white
                        px-6
                        py-3
                        rounded-xl
                        font-semibold
                        hover:bg-purple-700
                        transition
                        cursor-pointer
                        transition-all
                        duration-300
                        hover:scale-110
                        hover:text-purple-700
                        hover:bg-white
                        "
          >
            شروع همکاری
          </button>

          {/* Mobile Button */}

          <button
            onClick={() => setOpen(!open)}
            className="
                        md:hidden
                        text-gray-900
                        cursor-pointer
                        transition-all
                        duration-300
                        hover:scale-110
                        hover:text-purple-600
                        "
          >
            {open ? <X size={28} /> : <Menu size={28} />}
          </button>
        </div>

        {/* Mobile Menu */}

        {open && (
          <motion.div
            initial={{
              opacity: 0,
              height: 0,
            }}
            animate={{
              opacity: 1,
              height: "auto",
            }}
            className="
                            md:hidden
                            bg-white
                            border-t
                            py-6
                            px-6
                            flex
                            flex-col
                            gap-5
                            border-purple-400
                            "
          >
            <Link
              to="/"
              onClick={() => setOpen(false)}
              className="
                      relative
                      text-gray-700
                      font-medium
                      transition-colors
                      duration-300
                      hover:text-purple-600
                      group
                  "
            >
              خانه
              <span
                className="
                          absolute
                          -bottom-[1px]
                          left-1/2
                          h-[2px]
                          w-0
                          bg-gradient-to-r
                          bg-purple-600
                          to-green-400
                          transition-all
                          duration-300
                          -translate-x-1/2
                          group-hover:w-full
                      "
              />
            </Link>

            <Link
              to="/services"
              onClick={() => setOpen(false)}
              className="
                      relative
                      text-gray-700
                      font-medium
                      transition-colors
                      duration-300
                      hover:text-purple-600
                      group
                  "
            >
              خدمات
              <span
                className="
                          absolute
                          -bottom-[1px]
                          left-1/2
                          h-[2px]
                          w-0
                          bg-gradient-to-r
                          bg-purple-600
                          to-green-400
                          transition-all
                          duration-300
                          -translate-x-1/2
                          group-hover:w-full
                      "
              />
            </Link>

            <Link
              to="/portfolio"
              onClick={() => setOpen(false)}
              className="
                      relative
                      text-gray-700
                      font-medium
                      transition-colors
                      duration-300
                      hover:text-purple-600
                      group
                  "
            >
              نمونه کارها
              <span
                className="
                          absolute
                          -bottom-[1px]
                          left-1/2
                          h-[2px]
                          w-0
                          bg-gradient-to-r
                          bg-purple-600
                          to-green-400
                          transition-all
                          duration-300
                          -translate-x-1/2
                          group-hover:w-full
                      "
              />
            </Link>

            <Link
              to="/about"
              onClick={() => setOpen(false)}
              className="
                      relative
                      text-gray-700
                      font-medium
                      transition-colors
                      duration-300
                      hover:text-purple-600
                      group
                  "
            >
              درباره ما
              <span
                className="
                          absolute
                          -bottom-[1px]
                          left-1/2
                          h-[2px]
                          w-0
                          bg-gradient-to-r
                          bg-purple-600
                          to-green-400
                          transition-all
                          duration-300
                          -translate-x-1/2
                          group-hover:w-full
                      "
              />
            </Link>

            <Link
              to="/contact"
              onClick={() => setOpen(false)}
              className="
                      relative
                      text-gray-700
                      font-medium
                      transition-colors
                      duration-300
                      hover:text-purple-600
                      group
                  "
            >
              تماس
              <span
                className="
                          absolute
                          -bottom-[1px]
                          left-1/2
                          h-[2px]
                          w-0
                          bg-gradient-to-r
                          bg-purple-600
                          to-green-400
                          transition-all
                          duration-300
                          -translate-x-1/2
                          group-hover:w-full
                      "
              />
            </Link>

            <Link
              to="/contact"
              onClick={() => setOpen(false)}
              className="
                        bg-purple-600
                        text-white
                        px-6
                        py-3
                        rounded-xl
                        cursor-pointer
                        transition-all
                        duration-300
                        hover:scale-110
                        hover:text-white-600
                        text-center
                        "
            >
              شروع همکاری
            </Link>
          </motion.div>
        )}
      </Container>
    </motion.nav>
  );
}
